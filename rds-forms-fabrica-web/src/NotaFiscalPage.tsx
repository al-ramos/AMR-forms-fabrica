import { useState, useEffect, useMemo } from "react";
import { API, icons, type NotaFiscalAPI } from "./types";
import { Icon, Spinner, ErrorBox } from "./ui";

const thStyle: React.CSSProperties = {
    padding: "10px 16px", fontSize: 11, fontWeight: 700, color: "#6B7280",
    textTransform: "uppercase", letterSpacing: "0.08em", textAlign: "left",
    borderBottom: "1px solid #1F2937", background: "#0D1117",
};
const tdStyle: React.CSSProperties = {
    padding: "12px 16px", fontSize: 13, color: "#D1D5DB", whiteSpace: "nowrap",
};

function StatusNF({ cancelado }: { cancelado: number | null }) {
    const cancelada = cancelado === 1;
    return (
        <span style={{
            fontSize: 11, fontWeight: 700, padding: "3px 10px", borderRadius: 12,
            background: cancelada ? "rgba(239,68,68,0.12)" : "rgba(16,185,129,0.12)",
            color: cancelada ? "#EF4444" : "#10B981",
        }}>
            {cancelada ? "CANCELADA" : "ATIVA"}
        </span>
    );
}

function AmbienteBadge({ ambiente }: { ambiente: string | null }) {
    if (!ambiente) return <span style={{ color: "#4B5563", fontSize: 12 }}>—</span>;
    const prod = ambiente === "1";
    return (
        <span style={{
            fontSize: 11, fontWeight: 700, padding: "3px 10px", borderRadius: 12,
            background: prod ? "rgba(59,130,246,0.12)" : "rgba(245,158,11,0.12)",
            color: prod ? "#3B82F6" : "#F59E0B",
        }}>
            {prod ? "PRODUÇÃO" : "HOMOLOG"}
        </span>
    );
}

function DetalheNF({ nf, onClose }: { nf: NotaFiscalAPI; onClose: () => void }) {
    return (
        <div style={{
            marginTop: 20, background: "#111827",
            border: "1px solid #3B82F640", borderRadius: 8, padding: 24,
        }}>
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 20 }}>
                <div style={{ fontSize: 15, fontWeight: 800, color: "#F9FAFB" }}>
                    NF <span style={{ color: "#3B82F6", fontFamily: "'DM Mono', monospace" }}>
                        {nf.cdNotaFiscal} / {nf.cdSerNotaFiscal}
                    </span>
                </div>
                <button onClick={onClose} style={{ background: "none", border: "none", color: "#6B7280", cursor: "pointer", fontSize: 18 }}>✕</button>
            </div>

            <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr 1fr", gap: 16, marginBottom: 20 }}>
                {([
                    ["Filial", nf.noFilial ?? `Filial ${nf.cdFilial}`],
                    ["Cliente", nf.noCliente ?? "—"],
                    ["CNPJ Cliente", nf.cdCnpjCliente ?? "—"],
                    ["Data Emissão", nf.dtEmissaoNf ? new Date(nf.dtEmissaoNf).toLocaleDateString("pt-BR") : "—"],
                    ["Ficha Vinculada", nf.cdFicha ? `#${String(nf.cdFicha).padStart(4, "0")}` : "—"],
                    ["Valor", nf.vlTransmissao != null ? `R$ ${nf.vlTransmissao.toFixed(2)}` : "—"],
                ] as [string, string][]).map(([k, v]) => (
                    <div key={k}>
                        <div style={{ fontSize: 10, color: "#6B7280", fontWeight: 700, textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 4 }}>{k}</div>
                        <div style={{ fontSize: 13, color: "#D1D5DB", fontWeight: 500 }}>{v}</div>
                    </div>
                ))}
            </div>

            {nf.cdChaveNfe && (
                <div style={{ background: "#0D1117", borderRadius: 6, padding: "10px 14px", border: "1px solid #1F2937" }}>
                    <div style={{ fontSize: 10, color: "#6B7280", fontWeight: 700, textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 4 }}>
                        Chave NF-e
                    </div>
                    <div style={{ fontFamily: "'DM Mono', monospace", fontSize: 11, color: "#3B82F6", wordBreak: "break-all" }}>
                        {nf.cdChaveNfe}
                    </div>
                </div>
            )}

            {nf.cdProtocolo && (
                <div style={{ background: "#0D1117", borderRadius: 6, padding: "10px 14px", border: "1px solid #1F2937", marginTop: 10 }}>
                    <div style={{ fontSize: 10, color: "#6B7280", fontWeight: 700, textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 4 }}>
                        Protocolo
                    </div>
                    <div style={{ fontFamily: "'DM Mono', monospace", fontSize: 11, color: "#D1D5DB" }}>
                        {nf.cdProtocolo}
                    </div>
                </div>
            )}
        </div>
    );
}

export default function NotaFiscalPage() {
    const [nfs, setNfs] = useState<NotaFiscalAPI[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [search, setSearch] = useState("");
    const [selected, setSelected] = useState<NotaFiscalAPI | null>(null);
    const [filtroStatus, setFiltroStatus] = useState<"todos" | "ativa" | "cancelada">("todos");

    const load = async () => {
        setLoading(true);
        setError(null);
        try {
            const res = await fetch(`${API}/api/NotaFiscal`);
            if (!res.ok) throw new Error(`HTTP ${res.status}`);
            setNfs(await res.json());
        } catch (e: any) {
            setError(e.message ?? "Erro ao carregar notas fiscais");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => { load(); }, []);

    const filtradas = useMemo(() => nfs.filter(n => {
        const matchSearch =
            String(n.cdNotaFiscal).includes(search) ||
            (n.noCliente ?? "").toLowerCase().includes(search.toLowerCase()) ||
            (n.noFilial ?? "").toLowerCase().includes(search.toLowerCase()) ||
            (n.cdChaveNfe ?? "").includes(search);

        const matchStatus =
            filtroStatus === "todos" ? true :
                filtroStatus === "cancelada" ? n.icCancelado === 1 :
                    n.icCancelado !== 1;

        return matchSearch && matchStatus;
    }), [nfs, search, filtroStatus]);

    const totais = useMemo(() => ({
        total: nfs.length,
        ativas: nfs.filter(n => n.icCancelado !== 1).length,
        canceladas: nfs.filter(n => n.icCancelado === 1).length,
    }), [nfs]);

    return (
        <div>
            {/* Header */}
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start", marginBottom: 24 }}>
                <div>
                    <h1 style={{ fontSize: 22, fontWeight: 800, color: "#F9FAFB", margin: 0, letterSpacing: "-0.03em" }}>
                        Notas Fiscais
                    </h1>
                    <p style={{ fontSize: 13, color: "#6B7280", margin: "4px 0 0" }}>
                        {loading ? "Carregando..." : `${totais.total} notas no período`}
                    </p>
                </div>
                <button onClick={load} style={{
                    background: "#1F2937", border: "1px solid #374151", borderRadius: 6,
                    color: "#9CA3AF", fontSize: 12, padding: "9px 14px",
                    cursor: "pointer", display: "flex", alignItems: "center", gap: 6,
                }}>
                    <Icon d={icons.refresh} size={13} /> Atualizar
                </button>
            </div>

            {/* KPIs */}
            <div style={{ display: "grid", gridTemplateColumns: "repeat(3, 1fr)", gap: 16, marginBottom: 20 }}>
                {[
                    { label: "Total de NFs", valor: totais.total, cor: "#3B82F6" },
                    { label: "Ativas", valor: totais.ativas, cor: "#10B981" },
                    { label: "Canceladas", valor: totais.canceladas, cor: "#EF4444" },
                ].map(k => (
                    <div key={k.label} style={{
                        background: "#111827", borderRadius: 8, padding: "16px 20px",
                        borderLeft: `4px solid ${k.cor}`, border: `1px solid #1F2937`,
                        borderLeftWidth: 4, borderLeftColor: k.cor,
                    }}>
                        <div style={{ fontSize: 11, color: "#6B7280", fontWeight: 700, textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 6 }}>{k.label}</div>
                        <div style={{ fontSize: 32, fontWeight: 800, color: k.cor }}>{k.valor}</div>
                    </div>
                ))}
            </div>

            {/* Filtros */}
            <div style={{ display: "flex", gap: 10, marginBottom: 16, alignItems: "center" }}>
                <input
                    value={search}
                    onChange={e => setSearch(e.target.value)}
                    placeholder="Buscar por NF, cliente, filial ou chave..."
                    style={{
                        flex: 1, maxWidth: 400, background: "#111827", border: "1px solid #374151",
                        borderRadius: 6, color: "#D1D5DB", fontSize: 13, padding: "9px 14px", outline: "none",
                    }}
                />
                {(["todos", "ativa", "cancelada"] as const).map(f => (
                    <button key={f} onClick={() => setFiltroStatus(f)} style={{
                        background: filtroStatus === f ? "#1F2937" : "transparent",
                        border: `1px solid ${filtroStatus === f ? "#374151" : "#1F2937"}`,
                        borderRadius: 6, color: filtroStatus === f ? "#F9FAFB" : "#6B7280",
                        fontSize: 12, fontWeight: 600, padding: "8px 14px", cursor: "pointer",
                        textTransform: "capitalize",
                    }}>
                        {f === "todos" ? "Todos" : f === "ativa" ? "Ativas" : "Canceladas"}
                    </button>
                ))}
            </div>

            {/* Tabela */}
            {loading ? <Spinner /> : error ? <ErrorBox message={error} onRetry={load} /> : (
                <>
                    <div style={{ background: "#111827", border: "1px solid #1F2937", borderRadius: 8, overflow: "hidden" }}>
                        <table style={{ width: "100%", borderCollapse: "collapse" }}>
                            <thead>
                                <tr>
                                    {["NF / Série", "Filial", "Cliente", "Emissão", "Ficha", "Valor", "Ambiente", "Status"].map(h => (
                                        <th key={h} style={thStyle}>{h}</th>
                                    ))}
                                </tr>
                            </thead>
                            <tbody>
                                {filtradas.map(n => (
                                    <tr key={`${n.cdNotaFiscal}-${n.cdSerNotaFiscal}`}
                                        onClick={() => setSelected(s => s?.cdNotaFiscal === n.cdNotaFiscal ? null : n)}
                                        style={{
                                            cursor: "pointer",
                                            background: selected?.cdNotaFiscal === n.cdNotaFiscal ? "rgba(59,130,246,0.08)" : "transparent",
                                            borderBottom: "1px solid #1F2937", transition: "background 0.15s",
                                        }}
                                        onMouseEnter={e => { if (selected?.cdNotaFiscal !== n.cdNotaFiscal) e.currentTarget.style.background = "rgba(255,255,255,0.03)"; }}
                                        onMouseLeave={e => { if (selected?.cdNotaFiscal !== n.cdNotaFiscal) e.currentTarget.style.background = "transparent"; }}
                                    >
                                        <td style={tdStyle}>
                                            <span style={{ fontFamily: "'DM Mono', monospace", color: "#3B82F6", fontWeight: 700 }}>
                                                {n.cdNotaFiscal}
                                            </span>
                                            <span style={{ color: "#4B5563", fontSize: 11, marginLeft: 4 }}>/ {n.cdSerNotaFiscal}</span>
                                        </td>
                                        <td style={tdStyle}>{n.noFilial ?? `Filial ${n.cdFilial}`}</td>
                                        <td style={tdStyle}>{n.noCliente ?? "—"}</td>
                                        <td style={tdStyle}>
                                            {n.dtEmissaoNf ? new Date(n.dtEmissaoNf).toLocaleDateString("pt-BR") : "—"}
                                        </td>
                                        <td style={tdStyle}>
                                            {n.cdFicha
                                                ? <span style={{ fontFamily: "'DM Mono', monospace", color: "#E85D04", fontWeight: 700 }}>#{String(n.cdFicha).padStart(4, "0")}</span>
                                                : <span style={{ color: "#4B5563" }}>—</span>
                                            }
                                        </td>
                                        <td style={tdStyle}>
                                            {n.vlTransmissao != null
                                                ? <span style={{ fontFamily: "'DM Mono', monospace", color: "#10B981" }}>R$ {n.vlTransmissao.toFixed(2)}</span>
                                                : <span style={{ color: "#4B5563" }}>—</span>
                                            }
                                        </td>
                                        <td style={tdStyle}><AmbienteBadge ambiente={n.cdAmbiente} /></td>
                                        <td style={tdStyle}><StatusNF cancelado={n.icCancelado} /></td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                        {filtradas.length === 0 && (
                            <div style={{ padding: 40, textAlign: "center", color: "#6B7280", fontSize: 13 }}>
                                Nenhuma nota fiscal encontrada.
                            </div>
                        )}
                    </div>
                    {selected && <DetalheNF nf={selected} onClose={() => setSelected(null)} />}
                </>
            )}
        </div>
    );
}