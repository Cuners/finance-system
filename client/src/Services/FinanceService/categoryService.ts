import type { CategoryDto } from '../../Types';
import { httpService } from '../httpService';

const API_BASE = '/api/budet/Category';

export const categoryService = {
    getCategory(): Promise<CategoryDto[]> {
        return httpService.get(`${API_BASE}`);
    },
};