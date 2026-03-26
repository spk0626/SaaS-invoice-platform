export interface Customer {
    id: string;
    name: string;
    email: string;
    phone: string | null;
    address: string | null;
    isActive: boolean;
    createdAt: string; // ISO date string
}

export interface CreateCustomerRequest {
    name: string;
    email: string;
    phone?: string | null;
    address?: string | null;
}

export interface UpdateCustomerRequest {
    name: string;
    phone?: string | null;
    address?: string | null;
}



