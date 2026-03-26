export interface LoginRequest {
    email: string;
    password: string;
}

export interface RegisterRequest {
    email: string;
    password: string;
}

export interface AuthResponse {
    accessToken: string;
    refreshToken: string;
    expiresIn: number;
}

export interface TokenPayload {
    sub: string; // User ID
    email: string;
    role: string | string[]; // User role(s)
    exp: number; // Expiration time (timestamp)
    jti: string; // Unique identifier for the token
}