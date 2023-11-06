export interface Notification {
    notificationId: number;
    message: string;
    date: Date;
    type: string;
  }
  
  export interface State {
    exercises: Notification[];
  }
  