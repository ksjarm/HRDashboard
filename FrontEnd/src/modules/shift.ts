import { EmployeeShift } from './employeeShift';

export interface Shift {
  id: number;
  title: string;
  date: string;
  startTime: string;
  endTime: string;
  valik: string;
  startDate?: string;
  endDate?: string;
  selectedWeekDay?: string;
  employeeShifts?: EmployeeShift[];
  assignedEmployeesNames?: string[];
}

export interface State {
  shifts: Shift[];
}
