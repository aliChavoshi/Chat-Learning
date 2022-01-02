export interface IPagination {
  currentPage: number;
  totalPage: number;
  pageSize: number;
  totalCount: number;
}
export class PaginatedResult<T> {
  items: T;
  pagination: IPagination;
}
