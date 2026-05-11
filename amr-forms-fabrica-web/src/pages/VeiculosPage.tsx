import { useState, useEffect } from "react";
import { API } from "../types";

interface VeiculoAPI {
  placa: string;
  codigoFilial: number;
  ufVeiculo: string | null;
  codigoRntc: string | null;
}

const tdStyle: React.CSSProperties = {
  padding: "12px 16px", fontSize: 13, color: "#D1D5DB", whiteSpace: "nowrap",
};
const thStyle: React.CSSProperties = {
  padding: "10px 16px", fontSize: 11, fontWeight: 700, color: "#6B7280",
  textTransform: "uppercase", letterSpacing: "0.08em", textAlign: "left",
  borderBottom: "1px solid #1F2937", background: "#0D1117",
};

export default function VeiculosPage() {
  const [veiculos, setVeiculos] = useState<VeiculoAPI[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [search, setSearch] = useState("");

  const carregar = async () => {
    setLoading(true);
    setError(null);
    try {
      const res = await fetch(`${API}/api/Veiculo`);
      if (!res.ok) throw new Error(`HTTP ${res.status}`);
      const data = await res.json();
      setVeiculos(data);
    } catch (e: any) {
      setError(e.message ?? "Erro ao carregar dados");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { carregar(); }, []);

  const filtered = veiculos.filter(v =>
    v.placa.toLowerCase().includes(search.toLowerCase()) ||
    (v.ufVeiculo ?? "").toLowerCase().includes(search.toLowerCase()) ||
    (v.codigoRntc ?? "").toLowerCase().includes(search.toLowerCase())
  );

  const comUf   = veiculos.filter(v => v.ufVeiculo).length;
  const comRntc = veiculos.filter(v => v.codigoRntc).length;

  return (
    <div>
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start", marginBottom: 24 }}>
        <div>
          <h1 style={{ fontSize: 22, fontWeight: 800, color: "#F9FAFB", margin: 0, letterSpacing: "-0.03em" }}>
            Veículos
          </h1>
          <p style={{ fontSize: 13, color: "#6B7280", margin: "4px 0 0" }}>
            {loading ? "Carregando..." : `${veiculos.length} veículos cadastrados`}
          </p>
        </div>
        <div style={{ display: "flex", gap: 8 }}>
          <button onClick={carregar} style={{
            background: "#111827", border: "1px solid #1F2937", borderRadius: 6,
            color: "#9CA3AF", fontWeight: 600, fontSize: 13, padding: "9px 16px",
            cursor: "pointer", display: "flex", alignItems: "center", gap: 6,
          }}>
            ↻ Atualizar
          </button>
          <button style={{
            background: "linear-gradient(135deg, #E85D04, #9A3412)",
            border: "none", borderRadius: 6, color: "white",
            fontWeight: 700, fontSize: 13, padding: "9px 18px", cursor: "pointer",
          }}>
            + Novo Veículo
          </button>
        </div>
      </div>

      {!loading && !error && (
        <div style={{ display: "grid", gridTemplateColumns: "repeat(3, 1fr)", gap: 16, marginBottom: 24 }}>
          {[
            { label: "Total",    value: veiculos.length, color: "#E85D04" },
            { label: "Com UF",   value: comUf,           color: "#10B981" },
            { label: "Com RNTC", value: comRntc,         color: "#3B82F6" },
          ].map(k => (
            <div key={k.label} style={{
              background: "#111827", border: "1px solid #1F2937",
              borderRadius: 8, padding: "20px 24px", position: "relative", overflow: "hidden",
            }}>
              <div style={{
                position: "absolute", top: 0, left: 0, right: 0, height: 2,
                background: `linear-gradient(90deg, ${k.color}, transparent)`,
              }} />
              <div style={{ fontSize: 11, fontWeight: 600, color: "#6B7280",
                textTransform: "uppercase", letterSpacing: "0.1em", marginBottom: 8 }}>
                {k.label}
              </div>
              <div style={{ fontSize: 36, fontWeight: 800, color: "#F9FAFB",
                fontFamily: "'DM Mono', monospace", lineHeight: 1 }}>
                {k.value}
              </div>
            </div>
          ))}
        </div>
      )}

      {!loading && !error && (
        <div style={{ marginBottom: 16, maxWidth: 320 }}>
          <input
            value={search} onChange={e => setSearch(e.target.value)}
            placeholder="Buscar por placa, filial ou UF..."
            style={{
              width: "100%", background: "#111827", border: "1px solid #374151",
              borderRadius: 6, color: "#D1D5DB", fontSize: 13,
              padding: "9px 14px", outline: "none", boxSizing: "border-box",
            }}
          />
        </div>
      )}

      {loading && (
        <div style={{ background: "#111827", border: "1px solid #1F2937",
          borderRadius: 8, padding: 60, textAlign: "center", color: "#6B7280", fontSize: 13 }}>
          Carregando veículos...
        </div>
      )}

      {error && (
        <div style={{ background: "#111827", border: "1px solid #1F2937",
          borderRadius: 8, padding: 48, textAlign: "center" }}>
          <div style={{ fontSize: 28, marginBottom: 12 }}>⚠️</div>
          <div style={{ fontSize: 14, fontWeight: 700, color: "#EF4444", marginBottom: 8 }}>
            Erro ao carregar dados
          </div>
          <div style={{ fontSize: 13, color: "#6B7280", marginBottom: 20 }}>{error}</div>
          <button onClick={carregar} style={{
            background: "#1F2937", border: "1px solid #374151", borderRadius: 6,
            color: "#D1D5DB", fontWeight: 600, fontSize: 13, padding: "9px 18px",
            cursor: "pointer", display: "inline-flex", alignItems: "center", gap: 6,
          }}>
            ↻ Tentar novamente
          </button>
        </div>
      )}

      {!loading && !error && (
        <div style={{ background: "#111827", border: "1px solid #1F2937",
          borderRadius: 8, overflow: "hidden" }}>
          <table style={{ width: "100%", borderCollapse: "collapse" }}>
            <thead>
              <tr>
                {["Placa", "Filial", "UF", "RNTC"].map(h => (
                  <th key={h} style={thStyle}>{h}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {filtered.map(v => (
                <tr key={v.placa}
                  style={{ borderBottom: "1px solid #1F2937", cursor: "pointer" }}
                  onMouseEnter={e => (e.currentTarget.style.background = "rgba(255,255,255,0.03)")}
                  onMouseLeave={e => (e.currentTarget.style.background = "transparent")}
                >
                  <td style={tdStyle}>
                    <span style={{ fontFamily: "'DM Mono', monospace", fontSize: 13,
                      color: "#E85D04", fontWeight: 700 }}>{v.placa}</span>
                  </td>
                  <td style={tdStyle}>{v.codigoFilial}</td>
                  <td style={tdStyle}>
                    {v.ufVeiculo
                      ? <span style={{ fontSize: 11, fontWeight: 700, padding: "3px 10px",
                          borderRadius: 4, color: "#10B981", background: "rgba(16,185,129,0.12)",
                          border: "1px solid rgba(16,185,129,0.3)" }}>{v.ufVeiculo}</span>
                      : <span style={{ color: "#4B5563", fontSize: 12 }}>—</span>}
                  </td>
                  <td style={tdStyle}>
                    <span style={{ fontFamily: "'DM Mono', monospace", fontSize: 12, color: "#9CA3AF" }}>
                      {v.codigoRntc ?? "—"}
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          {filtered.length === 0 && (
            <div style={{ padding: 40, textAlign: "center", color: "#6B7280", fontSize: 13 }}>
              Nenhum veículo encontrado.
            </div>
          )}
        </div>
      )}
    </div>
  );
}
