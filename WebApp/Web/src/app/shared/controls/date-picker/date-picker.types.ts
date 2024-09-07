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