export interface Exercise {
  id: number;
  title: string;
  description: string;
  intensity: string;
  recommendedDurationInSeconds: number;
  recommendedRestTimeInSecondsBeforeExercies: number;
  recommendedRestTimeInSecondsAfterExercise: number;
  restTimeInstructions?: string;
  name: string;
  surname: string;
  email: string;
  phoneNumber: string;
  adress: string;
  salary: number;
  status: string;
}

export interface State {
  exercises: Exercise[];
}
