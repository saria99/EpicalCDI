// src/pages/DashboardPage.tsx
import React from 'react';
import { useQuery } from '@tanstack/react-query';
import { useAuthApi } from '../hooks/useAuthApi';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { Activity, Users, Database, AlertCircle } from 'lucide-react';

interface StatsSummary {
    totalPatients: number;
    totalEncounters: number;
    totalObservations: number;
}

export const DashboardPage: React.FC = () => {
    const { fetch } = useAuthApi();

    // Use Aspire proxy URL relative path since frontend is hosted via Aspire or has proxy setup
    // In local dev, Vite proxy needs configuration. 
    // Assuming process.env.services__api__https__0 is available via proxy or generic /api prefix if proxied
    const { data, isLoading, error } = useQuery<StatsSummary>({
        queryKey: ['stats'],
        queryFn: async () => {
            const res = await fetch('/api/stats/summary');
            if (!res.ok) throw new Error('Failed to fetch stats');
            return res.json();
        }
    });

    if (isLoading) return <div className="p-8">Loading stats...</div>;
    if (error) return <div className="p-8 text-red-600">Error loading dashboard data.</div>;

    const stats = data || { totalPatients: 0, totalEncounters: 0, totalObservations: 0 };

    const chartData = [
        { name: 'Patients', count: stats.totalPatients },
        { name: 'Encounters', count: stats.totalEncounters },
        { name: 'Observations', count: stats.totalObservations },
    ];

    return (
        <div className="space-y-6">
            {/* Stats Grid */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                <StatCard 
                    title="Total Encounters" 
                    value={stats.totalEncounters} 
                    icon={<Activity className="text-blue-600" />} 
                    trend="+12% from last week"
                />
                <StatCard 
                    title="Observations" 
                    value={stats.totalObservations} 
                    icon={<Database className="text-purple-600" />} 
                    trend="+5% from last week"
                />
                <StatCard 
                    title="Active Hospitals" 
                    value={3} // Hardcoded for now until we fetch hospital count
                    icon={<AlertCircle className="text-green-600" />} 
                    trend="All systems operational"
                />
                 <StatCard 
                    title="Total Patients" 
                    value={stats.totalPatients} 
                    icon={<Users className="text-orange-600" />} 
                    trend="+2 new today"
                />
            </div>

            {/* Chart */}
            <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-100">
                <h3 className="text-lg font-semibold text-gray-800 mb-6">Data Ingestion Overview</h3>
                <div className="h-80">
                    <ResponsiveContainer width="100%" height="100%">
                        <BarChart data={chartData}>
                            <CartesianGrid strokeDasharray="3 3" />
                            <XAxis dataKey="name" />
                            <YAxis />
                            <Tooltip />
                            <Legend />
                            <Bar dataKey="count" fill="#3b82f6" radius={[4, 4, 0, 0]} />
                        </BarChart>
                    </ResponsiveContainer>
                </div>
            </div>
        </div>
    );
};

const StatCard = ({ title, value, icon, trend }: { title: string, value: number, icon: React.ReactNode, trend: string }) => (
    <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-100 transition-hover hover:shadow-md">
        <div className="flex justify-between items-start mb-4">
            <div>
                <p className="text-sm font-medium text-gray-500">{title}</p>
                <h4 className="text-2xl font-bold text-gray-900 mt-1">{value.toLocaleString()}</h4>
            </div>
            <div className="p-2 bg-gray-50 rounded-lg">
                {icon}
            </div>
        </div>
        <p className="text-xs text-gray-400 font-medium">{trend}</p>
    </div>
);
