export interface IRequestLogin {
  userName: string;
  password: string;
}
export interface IRequestRegister {
  userName: string;
  password: string;
}
export interface User {
  userName: string;
  token: string;
  photoUrl: string;
}
