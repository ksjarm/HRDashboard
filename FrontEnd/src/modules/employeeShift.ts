import { Shift } from "./shift";

export interface EmployeeShift {
    employeeId: number;
    shiftId: number;
    shift: Shift;
  }
  
  export interface State {
    employeeShifts: EmployeeShift[];
  }
  
  
  
  
  
  