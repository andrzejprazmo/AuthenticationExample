
import { createCustomElement } from '@angular/elements';
import { importProvidersFrom, Injector } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { appConfig } from './app/app.config';
import { bootstrapApplication } from '@angular/platform-browser';

bootstrapApplication(AppComponent, {
  ...appConfig,
  providers: [
    ...(appConfig.providers || []),
    importProvidersFrom(BrowserModule)
  ]
}).then(appRef => {
  const injector = appRef.injector as Injector;
  const AppElement = createCustomElement(AppComponent, { injector });
  if (!customElements.get('external-api-client')) {
    customElements.define('external-api-client', AppElement);
  }
});
