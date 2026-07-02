export interface Jogador {
  id: string;
  nome: string;
}

export interface RegistroPartida {
  id: string;
  jogadorId: string;
  data: string; // ISO format YYYY-MM-DD
  gols: number;
  assistencias: number;
}

export interface EstatisticaJogador {
  nome: string;
  totalGols: number;
  totalAssistencias: number;
}

export interface CriarRegistroDto {
  jogadorId: string;
  data: string;
  gols: number;
  assistencias: number;
}

export interface AtualizarRegistroDto {
  gols: number;
  assistencias: number;
}
