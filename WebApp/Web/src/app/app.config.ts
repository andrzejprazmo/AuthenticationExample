import { APP_INITIALIZER, ApplicationConfig, FactoryProvider, InjectionToken, Provider, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { JWT_OPTIONS, JwtConfig, JwtModule } from '@auth0/angular-jwt';
import { HttpClient, provideHttpClient, withInterceptors, withInterceptorsFromDi } from '@angular/common/http';
import { authInterceptor } from '@shared/interceptors/auth.interceptor';
import { ModalModule, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrModule } from 'ngx-toastr';
import { firstValueFrom, tap } from 'rxjs';
import { APPLICATION_CONFIG, GlobalConfiguration } from '@shared/types/global.types';


const jwtOptionsProvider: Provider = {
  provide: JWT_OPTIONS,
  useFactory: () => {
    return {
      tokenGetter: () => sessionStorage.getItem('Authorization'),
      allowedDomains: [],
      disallowedRoutes: [],
      }
  },
}

export function initializeApp(http: HttpClient, jwtConfig: JwtConfig, appConfig: GlobalConfiguration) {
  return (): Promise<any> =>
    firstValueFrom(
      http
        .get<GlobalConfiguration>('/api/config')
        .pipe(tap(config => {
          jwtConfig.allowedDomains?.push(...config.baseDomain);
          appConfig.baseDomain = config.baseDomain
        }))
    );
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideHttpClient(withInterceptorsFromDi()),
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      multi: true,
      deps: [HttpClient, JWT_OPTIONS, APPLICATION_CONFIG],
    },
    {
      provide: APPLICATION_CONFIG,
      useValue: {
        baseDomain: ''
      }
    },
    importProvidersFrom([
      JwtModule.forRoot({ jwtOptionsProvider }),
      ModalModule,
      ToastrModule.forRoot()
    ])
  ]
};
