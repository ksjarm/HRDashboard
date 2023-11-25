export interface User {
  id: number;
  name: string;
  surname: string;
  phoneNumber: string;
  adress: string;
  username: string;
  password: string;
  role: string;
  rememberMe: boolean;
}

export interface AuthResponse {
  token: string;
}
