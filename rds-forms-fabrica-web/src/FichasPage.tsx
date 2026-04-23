import { useState, useMemo, memo } from "react";
import { statusMap, icons, type Ficha } from './types';
import { passos, tdStyle, thStyle } from "./types";
import { Icon, StatusBadge, PassoTimeline, Spinner, ErrorBox } from "./ui";

// ── Detalhe da ficha selecionada — memo: só re-renderiza se selected mudar ────
const FichaDetalhe = memo(({ ficha, onClose }: { ficha: Ficha; onClose: () => void }) => (
    <div style={{ marginTop: 20, background: "#111827", border: "1px solid #E85D0440", borderRadius: 8, padding: 24 }}>
        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 16 }}>
            <div style={{ fontSize: 15, fontWeight: 800, color: "#F9FAFB" }}>
                Ficha <span style={{ color: "#E85D04", fontFamily: "'DM Mono', monospace" }}>#{String(ficha.cdFicha).padStart(4, "0")}</span>
            </div>
            <button onClick={onClose} style={{ background: "none", border: "none", color: "#6B7280", cursor: "pointer", fontSize: 18 }}>✕</button>
        </div>
        <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr 1fr", gap: 16 }}>
            {([
                ["Filial", ficha.noFilial],
                ["Tipo de Operação", ficha.noTipoOp],
                ["Veículo", ficha.placa],
                ["Data", new Date(ficha.dtFicha).toLocaleDateString("pt-BR")],
                ["Passo Atual", ficha.noPasso],
                ["Status", statusMap[ficha.status].label],
            ] as [string, string][]).map(([k, v]) => (
                <div key={k}>
                    <div style={{ fontSize: 10, color: "#6B7280", fontWeight: 700, textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 4 }}>{k}</div>
                    <div style={{ fontSize: 13, color: "#D1D5DB", fontWeight: 500 }}>{v}</div>
                </div>
            ))}
        </div>
        <div style={{ marginTop: 20 }}>
            <div style={{ fontSize: 11, color: "#6B7280", fontWeight: 700, textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 12 }}>
                Timeline de Passos
            </div>
            <div style={{ display: "flex", alignItems: "center" }}>
                {passos.map((p, i) => {
                    const idx = passos.indexOf(ficha.noPasso as typeof passos[number]);
                    const done = i < idx, current = i === idx;
                    return (
                        <div key={p} style={{ display: "flex", alignItems: "center", flex: 1 }}>
                            <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: 6 }}>
                                <div style={{
                                    width: 28, height: 28, borderRadius: "50%",
                                    background: done ? "#E85D04" : current ? "transparent" : "#1F2937",
                                    border: current ? "2px solid #E85D04" : done ? "none" : "2px solid #374151",
                                    display: "flex", alignItems: "center", justifyContent: "center",
                                    fontSize: 11, fontWeight: 700, color: done ? "white" : current ? "#E85D04" : "#6B7280",
                                    boxShadow: current ? "0 0 12px #E85D0460" : "none"
                                }}>
                                    {done ? "✓" : i + 1}
                                </div>
                                <span style={{
                                    fontSize: 10, whiteSpace: "nowrap",
                                    color: current ? "#F97316" : done ? "#D1D5DB" : "#4B5563",
                                    fontWeight: current ? 700 : 500
                                }}>{p}</span>
                            </div>
                            {i < passos.length - 1 && (
                                <div style={{ flex: 1, height: 2, margin: "0 4px", marginBottom: 20, background: i < idx ? "#E85D04" : "#1F2937" }} />
                            )}
                        </div>
                    );
                })}
            </div>
        </div>
    </div>
));

// ── FichasPage ────────────────────────────────────────────────────────────────
export default function FichasPage({ fichas, loading, error, reload, onNova }: {
    fichas: Ficha[]; loading: boolean; error: string | null; reload: () => void; onNova: () => void;
}) {
    const [selected, setSelected] = useState<Ficha | null>(null);
    const [search, setSearch] = useState("");

    // useMemo: recalcula o filtro só quando fichas ou search mudam
    const filtered = useMemo(() =>
        fichas.filter(f =>
            f.noFilial.toLowerCase().includes(search.toLowerCase()) ||
            f.noTipoOp.toLowerCase().includes(search.toLowerCase()) ||
            f.placa.toLowerCase().includes(search.toLowerCase()) ||
            String(f.cdFicha).includes(search)
        ),
        [fichas, search]
    );

    return (
        <div>
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start", marginBottom: 24 }}>
                <div>
                    <h1 style={{ fontSize: 22, fontWeight: 800, color: "#F9FAFB", margin: 0, letterSpacing: "-0.03em" }}>Fichas de Operação</h1>
                    <p style={{ fontSize: 13, color: "#6B7280", margin: "4px 0 0" }}>
                        {loading ? "Carregando..." : `${fichas.length} fichas no período`}
                    </p>
                </div>
                <div style={{ display: "flex", gap: 8 }}>
                    <button onClick={reload} style={{
                        background: "#1F2937", border: "1px solid #374151",
                        borderRadius: 6, color: "#9CA3AF", fontSize: 12, padding: "9px 14px",
                        cursor: "pointer", display: "flex", alignItems: "center", gap: 6
                    }}>
                        <Icon d={icons.refresh} size={13} /> Atualizar
                    </button>
                    <button onClick={onNova} style={{
                        background: "linear-gradient(135deg, #E85D04, #9A3412)",
                        border: "none", borderRadius: 6, color: "white", fontWeight: 700,
                        fontSize: 13, padding: "9px 18px", cursor: "pointer"
                    }}>
                        + Nova Ficha
                    </button>
                </div>
            </div>

            <div style={{ marginBottom: 16, maxWidth: 320 }}>
                <input value={search} onChange={e => setSearch(e.target.value)}
                    placeholder="Buscar por ficha, filial, placa..."
                    style={{
                        width: "100%", background: "#111827", border: "1px solid #374151",
                        borderRadius: 6, color: "#D1D5DB", fontSize: 13, padding: "9px 14px",
                        outline: "none", boxSizing: "border-box"
                    }} />
            </div>

            {loading ? <Spinner /> : error ? <ErrorBox message={error} onRetry={reload} /> : (
                <>
                    <div style={{ background: "#111827", border: "1px solid #1F2937", borderRadius: 8, overflow: "hidden" }}>
                        <table style={{ width: "100%", borderCollapse: "collapse" }}>
                            <thead>
                                <tr>{["Ficha", "Filial", "Data", "Tipo Op.", "Progresso", "Veículo", "Status"].map(h => <th key={h} style={thStyle}>{h}</th>)}</tr>
                            </thead>
                            <tbody>
                                {filtered.map(f => (
                                    <tr key={f.cdFicha} onClick={() => setSelected(f)}
                                        style={{
                                            cursor: "pointer",
                                            background: selected?.cdFicha === f.cdFicha ? "rgba(232,93,4,0.08)" : "transparent",
                                            borderBottom: "1px solid #1F2937", transition: "background 0.15s"
                                        }}
                                        onMouseEnter={e => { if (selected?.cdFicha !== f.cdFicha) e.currentTarget.style.background = "rgba(255,255,255,0.03)"; }}
                                        onMouseLeave={e => { if (selected?.cdFicha !== f.cdFicha) e.currentTarget.style.background = "transparent"; }}>
                                        <td style={tdStyle}><span style={{ fontFamily: "'DM Mono', monospace", fontSize: 13, color: "#E85D04", fontWeight: 700 }}>#{String(f.cdFicha).padStart(4, "0")}</span></td>
                                        <td style={tdStyle}>{f.noFilial}</td>
                                        <td style={tdStyle}>{new Date(f.dtFicha).toLocaleDateString("pt-BR")}</td>
                                        <td style={tdStyle}>{f.noTipoOp}</td>
                                        <td style={tdStyle}><div style={{ display: "flex", alignItems: "center", gap: 8 }}><PassoTimeline passoAtual={f.noPasso} /><span style={{ fontSize: 12, color: "#9CA3AF" }}>{f.noPasso}</span></div></td>
                                        <td style={tdStyle}><span style={{ fontFamily: "'DM Mono', monospace", fontSize: 12, color: "#D1D5DB" }}>{f.placa}</span></td>
                                        <td style={tdStyle}><StatusBadge status={f.status} /></td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                        {filtered.length === 0 && <div style={{ padding: 40, textAlign: "center", color: "#6B7280", fontSize: 13 }}>Nenhuma ficha encontrada.</div>}
                    </div>
                    {selected && <FichaDetalhe ficha={selected} onClose={() => setSelected(null)} />}
                </>
            )}
        </div>
    );
}
