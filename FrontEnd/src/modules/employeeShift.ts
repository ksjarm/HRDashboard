import { Employee } from './employee';
import { Shift } from './shift';

export interface EmployeeShift {
  employeeId: number;
  shiftId: number;
  employee: Employee | null;
  shift: Shift | null;
}

export interface State {
  employeeShifts: EmployeeShift[];
}
