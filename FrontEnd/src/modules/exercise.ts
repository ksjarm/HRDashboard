export interface Exercise {
  id: number;
  title: string;
  description: string;
  intensity: string;
  recommendedDurationInSeconds: number;
  recommendedRestTimeInSecondsBeforeExercies: number;
  recommendedRestTimeInSecondsAfterExercise: number;
  restTimeInstructions?: string;
}

export interface State {
  exercises: Exercise[];
}
