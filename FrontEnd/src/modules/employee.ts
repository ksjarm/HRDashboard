import { EmployeeShift } from "./employeeShift";

export interface Employee {
  id: number;
  name: string;
  surname: string;
  gender: string;
  dateOfBirth: string;
  email: string;
  phoneNumber: string;
  adress: string;
  position: string;
  salary: number;
  status: string;
  photo: string;
  shifts?: EmployeeShift[];
}

export interface State {
  employees: Employee[];
}
