// src/hooks/useAuthApi.ts
import { useAuth } from "react-oidc-context";
import { useMemo } from "react";

export function useAuthApi() {
    const auth = useAuth();
    
    const api = useMemo(() => {
        const fetchWithAuth = async (input: RequestInfo | URL, init?: RequestInit) => {
            const token = auth.user?.access_token;
            const headers = new Headers(init?.headers);
            
            if (token) {
                headers.set("Authorization", `Bearer ${token}`);
            }

            const config: RequestInit = {
                ...init,
                headers,
            };

            const response = await fetch(input, config);

            if (response.status === 401) {
                // Determine behavior on 401 - logout or refresh?
                // auth.signinSilent(); 
            }

            return response;
        };

        return { fetch: fetchWithAuth };
    }, [auth.user?.access_token]);

    return api;
}
