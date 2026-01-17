// src/pages/HospitalsPage.tsx
import React, { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { useAuthApi } from '../hooks/useAuthApi';
import { Plus, Search, MoreHorizontal } from 'lucide-react';

interface Hospital {
    id: { value: string };
    hospitalCode: string;
    name: string;
    timeZone: string;
    externalSystemType: number;
    status: number;
    createdUtc: string;
}

export const HospitalsPage: React.FC = () => {
    const { fetch } = useAuthApi();
    const [searchTerm, setSearchTerm] = useState('');

    const { data: hospitals, isLoading, error } = useQuery<Hospital[]>({
        queryKey: ['hospitals'],
        queryFn: async () => {
            const res = await fetch('/api/hospitals');
             if (!res.ok) throw new Error('Failed to fetch hospitals');
            return res.json();
        }
    });

    if (isLoading) return <div className="p-8">Loading hospitals...</div>;
    if (error) return <div className="p-8 text-red-600">Error loading hospitals.</div>;

    const filteredHospitals = hospitals?.filter(h => 
        h.name.toLowerCase().includes(searchTerm.toLowerCase()) || 
        h.hospitalCode.toLowerCase().includes(searchTerm.toLowerCase())
    ) || [];

    return (
        <div className="space-y-6">
            {/* Toolbar */}
            <div className="flex justify-between items-center bg-white p-4 rounded-xl shadow-sm border border-gray-100">
                <div className="relative w-96">
                    <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" size={18} />
                    <input 
                        type="text" 
                        placeholder="Search hospitals..." 
                        value={searchTerm}
                        onChange={e => setSearchTerm(e.target.value)}
                        className="w-full pl-10 pr-4 py-2 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all"
                    />
                </div>
                <button className="flex items-center gap-2 bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg font-medium transition-colors">
                    <Plus size={18} />
                    Onboard Hospital
                </button>
            </div>

            {/* List */}
            <div className="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
                <table className="w-full text-left">
                    <thead className="bg-gray-50 border-b border-gray-100">
                        <tr>
                            <th className="px-6 py-4 text-xs font-semibold text-gray-500 uppercase">Hospital Name</th>
                            <th className="px-6 py-4 text-xs font-semibold text-gray-500 uppercase">Code</th>
                            <th className="px-6 py-4 text-xs font-semibold text-gray-500 uppercase">Status</th>
                            <th className="px-6 py-4 text-xs font-semibold text-gray-500 uppercase">System</th>
                            <th className="px-6 py-4 text-xs font-semibold text-gray-500 uppercase">Created</th>
                            <th className="px-6 py-4 w-10"></th>
                        </tr>
                    </thead>
                    <tbody className="divide-y divide-gray-100">
                        {filteredHospitals.map(hospital => (
                            <tr key={hospital.id.value} className="hover:bg-gray-50/50 transition-colors">
                                <td className="px-6 py-4">
                                    <div className="font-medium text-gray-900">{hospital.name}</div>
                                    <div className="text-xs text-gray-400">{hospital.timeZone}</div>
                                </td>
                                <td className="px-6 py-4">
                                    <span className="font-mono text-sm bg-gray-100 px-2 py-1 rounded text-gray-600">
                                        {hospital.hospitalCode}
                                    </span>
                                </td>
                                <td className="px-6 py-4">
                                    <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium 
                                        ${hospital.status === 1 ? 'bg-green-100 text-green-800' : 'bg-yellow-100 text-yellow-800'}`}>
                                        {hospital.status === 1 ? 'Active' : 'Onboarding'}
                                    </span>
                                </td>
                                <td className="px-6 py-4 text-sm text-gray-600">
                                    {getSystemType(hospital.externalSystemType)}
                                </td>
                                <td className="px-6 py-4 text-sm text-gray-500">
                                    {new Date(hospital.createdUtc).toLocaleDateString()}
                                </td>
                                <td className="px-6 py-4 text-right">
                                    <button className="text-gray-400 hover:text-gray-600">
                                        <MoreHorizontal size={18} />
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

// Helper for enum mapping
const getSystemType = (type: number) => {
    switch(type) {
        case 0: return 'Epic';
        case 1: return 'Cerner';
        case 2: return 'Meditech';
        default: return 'Other';
    }
}
