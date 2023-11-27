import { Employee } from './employee';
import { Shift } from './shift';

export interface EmployeeShift {
  employeeId: number;
  shiftId: number;
  employee?: Employee;
  shift: Shift;
}

export interface State {
  employeeShifts: EmployeeShift[];
}
