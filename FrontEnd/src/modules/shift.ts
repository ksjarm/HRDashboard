
export interface Shift {
    id: number;
    title: string;
    date: string;
    startTime: string;
    endTime: string;
    //employeeShifts: EmployeeShift
  }
  
  export interface State {
    shifts: Shift[];
  }
  