export interface IPagination {
  currentPage: number;
  totalPage: number;
  pageSize: number;
  totalCount: number;
}
export class PaginatedResult<T> implements IPagination {
  currentPage: number;
  totalPage: number;
  pageSize: number;
  totalCount: number;
  items: T;
}
