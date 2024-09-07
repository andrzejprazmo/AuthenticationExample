export interface Month {
    fullName: string;
    shortName: string;
    daysCount: number;
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
    weeks: Week[];
}