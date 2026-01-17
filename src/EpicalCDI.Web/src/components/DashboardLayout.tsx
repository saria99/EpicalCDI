// src/components/DashboardLayout.tsx
import React from 'react';
import { useAuth } from 'react-oidc-context';
import { Link, Outlet, useLocation } from 'react-router-dom';
import { LayoutDashboard, Building2, LogOut, User } from 'lucide-react';

export const DashboardLayout: React.FC = () => {
    const auth = useAuth();
    const location = useLocation();

    return (
        <div className="flex h-screen bg-gray-100">
            {/* Sidebar */}
            <aside className="w-64 bg-slate-900 text-white flex flex-col">
                <div className="p-6 border-b border-slate-700">
                    <h1 className="text-xl font-bold tracking-wider">Epical CDI</h1>
                    <p className="text-xs text-slate-400 mt-1">Clinical Data Ingestion</p>
                </div>

                <nav className="flex-1 p-4 space-y-2">
                    <NavLink to="/" icon={<LayoutDashboard size={20} />} label="Dashboard" active={location.pathname === '/'} />
                    <NavLink to="/hospitals" icon={<Building2 size={20} />} label="Hospitals" active={location.pathname.startsWith('/hospitals')} />
                </nav>

                <div className="p-4 border-t border-slate-700 bg-slate-800/50">
                    <div className="flex items-center gap-3 mb-3">
                        <div className="bg-blue-600 p-2 rounded-full">
                            <User size={16} />
                        </div>
                        <div className="overflow-hidden">
                            <p className="text-sm font-medium truncate">{auth.user?.profile.preferred_username || 'User'}</p>
                            <p className="text-xs text-slate-400 truncate">{auth.user?.profile.email}</p>
                        </div>
                    </div>
                    <button 
                        onClick={() => auth.signoutRedirect()}
                        className="w-full flex items-center justify-center gap-2 bg-slate-700 hover:bg-slate-600 text-sm py-2 px-4 rounded transition-colors"
                    >
                        <LogOut size={16} />
                        Sign Out
                    </button>
                </div>
            </aside>

            {/* Main Content */}
            <main className="flex-1 overflow-auto">
                <header className="bg-white shadow-sm px-8 py-4 flex justify-between items-center">
                    <h2 className="text-2xl font-semibold text-gray-800">
                         {location.pathname === '/' ? 'Overview' : location.pathname.split('/')[1].charAt(0).toUpperCase() + location.pathname.split('/')[1].slice(1)}
                    </h2>
                </header>
                <div className="p-8">
                    <Outlet />
                </div>
            </main>
        </div>
    );
};

const NavLink = ({ to, icon, label, active }: { to: string, icon: React.ReactNode, label: string, active: boolean }) => (
    <Link 
        to={to} 
        className={`flex items-center gap-3 px-4 py-3 rounded-lg transition-all ${
            active 
                ? 'bg-blue-600 text-white shadow-md' 
                : 'text-slate-300 hover:bg-slate-800 hover:text-white'
        }`}
    >
        {icon}
        <span className="font-medium">{label}</span>
    </Link>
);
