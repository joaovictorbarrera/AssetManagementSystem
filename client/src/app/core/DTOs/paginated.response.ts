import Pagination from "./pagination";

export default interface PaginatedResponse<T> {
    items: T[],
    pagination: Pagination
}

export const defaultPaginatedResponse = <T>(): PaginatedResponse<T> => ({
  items: [],
  pagination: {
    pageNumber: 0,
    pageSize: 0,
    totalCount: 0,
    totalPages: 0,
    hasPreviousPage: false,
    hasNextPage: false,
  },
});
