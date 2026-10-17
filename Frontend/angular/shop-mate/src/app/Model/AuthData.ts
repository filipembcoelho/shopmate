export interface AuthUser {
  id: number;
  fullName: string;
  username: string;
  role: string;
}

export interface LoginResponse {
  accessToken: string;
  expiresAtUtc: string;
  user: AuthUser;
}
