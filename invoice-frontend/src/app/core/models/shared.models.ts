export interface PagedResult<T> {
    items: T[];
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
    hasNextPage: boolean;
    hasPreviousPage: boolean;
}

export interface ApiError {
    status: number;
    error: string;
    errors?: string[]; // Optional array of error messages
    traceId?: string; // Optional trace ID for debugging
    timestamp: string; // Timestamp of the error occurrence
}

