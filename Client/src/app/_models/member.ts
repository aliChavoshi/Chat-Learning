export interface IMember {
  id: number;
  userName: string;
  email: string;
  birthday: string;
  lastActive: string;
  knowAs: string;
  city: string;
  country: string;
  photoUrl: string;
  age: number;
  photos: Photo[];
}
export interface Photo {
  id: number;
  url: string;
  isMain: boolean;
}
