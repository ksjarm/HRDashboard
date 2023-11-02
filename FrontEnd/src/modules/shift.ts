import { EmployeeShift } from "./employeeShift";

export interface Shift {
    id: number;
    title: string;
    date: Date;
    startTime: String;
    endTime: String;
    employeeShifts: EmployeeShift 
  }
  
  export interface State {
    shifts: Shift[];
  }
  