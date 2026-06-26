export default interface Pagination {
    pageNumber: number,
    pageSize: number,
    totalCount: number,
    totalPages: number,
    hasPreviousPage: boolean,
    hasNextPage: boolean
}
