import { InjectionToken } from "@angular/core";

export interface GlobalConfiguration {
    baseDomain: string,
}

export const APPLICATION_CONFIG = new InjectionToken<GlobalConfiguration>('GlobalConfiguration');

