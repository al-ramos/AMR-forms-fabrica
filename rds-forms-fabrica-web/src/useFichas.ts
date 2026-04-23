import { useState, useEffect, useCallback } from "react";
import { API, PASSO_FINAL, type FichaAPI, type Ficha } from "./types";

function mapFicha(f: FichaAPI): Ficha {
  const noTipoOp =
    f.tipoOperacaos?.[0]?.noTipoOperacao ??
    ({ 1: "Carga", 2: "Descarga", 3: "Transferência" }[f.cdTipoOperacao] ?? `Op. ${f.cdTipoOperacao}`);

  const noPasso =
    f.cdPassoAtualNavigation?.noPasso ??
    ({ 1: "Entrada", 2: "Pesagem", 3: "Carregamento", 4: "Saída" }[f.cdPassoAtual] ?? `Passo ${f.cdPassoAtual}`);

  const status: Ficha["status"] =
    f.cdPassoAtual === 0 ? "aguardando"
    : f.cdPassoAtual >= PASSO_FINAL ? "concluida"
    : "em_andamento";

  return {
    cdFicha: f.cdFicha,
    noFilial: f.noFilial ?? `Filial ${f.cdFilial}`,
    dtFicha: f.dtFicha,
    noTipoOp,
    noPasso,
    placa: f.cdPlacaVeiculo ?? "—",
    status,
  };
}

export function useFichas() {
  const [fichas, setFichas] = useState<Ficha[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // useCallback garante referência estável — não recria a função em cada render
  const load = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const res = await fetch(`${API}/api/Ficha`);
      if (!res.ok) throw new Error(`HTTP ${res.status}`);
      const data: FichaAPI[] = await res.json();
      const seen = new Set<number>();
      setFichas(
        data
          .filter(f => { if (seen.has(f.cdFicha)) return false; seen.add(f.cdFicha); return true; })
          .map(mapFicha)
      );
    } catch (e: any) {
      setError(e.message ?? "Erro ao carregar fichas");
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { load(); }, [load]);

  return { fichas, loading, error, reload: load };
}
