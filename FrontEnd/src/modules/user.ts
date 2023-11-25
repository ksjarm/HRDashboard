export interface User {
  username: string;
  password: string;
  rememberMe: boolean;
}

export interface AuthResponse {
  token: string;
}
