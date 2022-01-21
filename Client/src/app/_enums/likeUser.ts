export enum PredicateLikeEnum {
  Liked,
  LikeBy,
}
export class UserLikeParams {
  pageNumber: number = 1;
  pageSize: number = 6;
  PredicateUserLike: PredicateLikeEnum = PredicateLikeEnum.Liked;
}
