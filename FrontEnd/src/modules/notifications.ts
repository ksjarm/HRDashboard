export interface Notifications {
    NotificationId: number;
    Message: string;
    Date: Date;
    Type: string;
  }
  
  export interface State {
    exercises: Notifications[];
  }
  