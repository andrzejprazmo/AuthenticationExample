import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { JwtModule } from '@auth0/angular-jwt';
import { provideHttpClient, withInterceptors, withInterceptorsFromDi } from '@angular/common/http';
import { authInterceptor } from '@shared/interceptors/auth.interceptor';
import { ModalModule, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrModule } from 'ngx-toastr';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideHttpClient(withInterceptorsFromDi()),
    importProvidersFrom([
      JwtModule.forRoot({
        config: {
          tokenGetter: () => {
            return sessionStorage.getItem('Authorization');
          },
          allowedDomains: [
            '*'
          ]
        }
      }),
      ModalModule,
      ToastrModule.forRoot()
    ])
  ]
};
