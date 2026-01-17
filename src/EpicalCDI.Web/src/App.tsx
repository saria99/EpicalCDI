import React from 'react';
import { AuthProvider, useAuth } from 'react-oidc-context';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { authConfig } from './authConfig';
import { DashboardLayout } from './components/DashboardLayout';
import { DashboardPage } from './pages/DashboardPage';
import { HospitalsPage } from './pages/HospitalsPage';

const queryClient = new QueryClient();

function App() {
  return (
    <AuthProvider {...authConfig}>
      <QueryClientProvider client={queryClient}>
        <BrowserRouter>
          <AuthWrapper>
             <Routes>
                <Route path="/" element={<DashboardLayout />}>
                    <Route index element={<DashboardPage />} />
                    <Route path="hospitals" element={<HospitalsPage />} />
                </Route>
             </Routes>
          </AuthWrapper>
        </BrowserRouter>
      </QueryClientProvider>
    </AuthProvider>
  );
}

// Protected Route Wrapper
const AuthWrapper: React.FC<{children: React.ReactNode}> = ({ children }) => {
    const auth = useAuth();
    
    if (auth.isLoading) {
        return <div className="h-screen flex items-center justify-center text-gray-500">Initializing Auth...</div>;
    }

    if (auth.error) {
        return <div className="h-screen flex items-center justify-center text-red-500">Auth Error: {auth.error.message}</div>;
    }

    if (!auth.isAuthenticated) {
        return (
            <div className="h-screen flex flex-col items-center justify-center bg-gray-50">
                 <h1 className="text-3xl font-bold text-blue-800 mb-8">Epical CDI</h1>
                 <button 
                    onClick={() => auth.signinRedirect()}
                    className="bg-blue-600 text-white px-6 py-3 rounded-lg font-semibold shadow hover:bg-blue-700 transition"
                >
                    Sign In with SSO
                 </button>
            </div>
        );
    }

    return <>{children}</>;
};

export default App;