export interface Month {
    index: number;
    fullName: string;
    shortName: string;
}
export interface WeekDay {
    fullName: string;
    shortName: string;
}
export interface Day {
    day: number;
    month: number;
    year: number;
    holiday: boolean;
    selected: boolean;
}
export interface Week {
    days: Day[];
}
export interface CalendarData {
    year: number;
    month: number;
    day: number;
    monthTable: Week[];
}