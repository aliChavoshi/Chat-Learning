export interface PaginationHeader {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}
export interface PaginationBody {
  currentPage: number;
  totalPage: number;
  pageSize: number;
  totalCount: number;
}
export class PaginatedResult<T> {
  result: T;
  paginationBody: PaginationBody;
  paginationHeader?: PaginationHeader;
}
