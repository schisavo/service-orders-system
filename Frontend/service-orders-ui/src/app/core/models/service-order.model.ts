export interface ServiceOrder {
  id: number;
  createdAt: string;
  status: string;
  description: string;
  technicianId: number;
  clientId: number;
}
