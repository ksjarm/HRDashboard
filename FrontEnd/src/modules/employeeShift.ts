import { Shift } from './shift';
import { Employee } from './employee';

export interface EmployeeShift {
  employeeId: number;
  shiftId: number;
  shift: Shift;
  employee: Employee;
}

export interface State {
  employeeShifts: EmployeeShift[];
}
