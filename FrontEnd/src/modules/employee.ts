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
}

export interface State {
  employees: Employee[];
}
