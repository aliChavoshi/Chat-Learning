export interface IMember {
  id: number;
  userName: string;
  photoUrl: string;
  lookingFor: string;
  knownAs: string;
  introduction: string;
  interests: string;
  email: string;
  country: string;
  city: string;
  gender: number; // 0 : female || 1 : male
  age: number;
  lastActive: string;
  dateOfBirth: string;
  created: string;
  photos: Photo[];
}
export interface Photo {
  id: number;
  url: string;
  isMain: boolean;
}
