import { EmployeeShift } from "./employeeShift";

export interface Shift {
    id: number;
    title: string;
    date: Date;
    startTime: Date;
    endTime: Date;
    employeeShifts: EmployeeShift 
  }
  
  export interface State {
    shifts: Shift[];
  }
  