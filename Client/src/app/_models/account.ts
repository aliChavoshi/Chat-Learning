export interface IRequestLogin {
  userName: string;
  password: string;
}
export interface IRequestRegister {
  userName: string;
  password: string;
  gender: number;
  dateOfBirth: string;
  knownAs: string;
  city: string;
  country: string;
}
export interface IUser {
  userName: string;
  token: string;
  photoUrl: string;
  gender: number;
  knownAs: string;
  roles?: string[];
}
