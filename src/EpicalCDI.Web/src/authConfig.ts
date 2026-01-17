// src/authConfig.ts
import { AuthProviderProps } from "react-oidc-context";

export const authConfig: AuthProviderProps = {
  authority: "http://localhost:8080/realms/epicalcdi", // Keycloak Realm URL
  client_id: "epicalcdi-web", // Client ID for the frontend
  redirect_uri: window.location.origin,
  onSigninCallback: () => {
    window.history.replaceState({}, document.title, window.location.pathname);
  },
  // Keycloak requires these defaults for code flow
  response_type: "code",
  scope: "openid profile email offline_access",
};
