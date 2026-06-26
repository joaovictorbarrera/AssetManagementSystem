import Pagination from "./pagination";

export default interface PaginatedResponse<T> {
    items: T[],
    pagination: Pagination
}

export const defaultPaginatedResponse = <T>(): PaginatedResponse<T> => ({
  items: [],
  pagination: {
    pageNumber: 1,
    pageSize: 1,
    totalCount: 1,
    totalPages: 1,
    hasPreviousPage: false,
    hasNextPage: false,
  },
});
