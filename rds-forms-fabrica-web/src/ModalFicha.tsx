import { useState, useEffect } from "react";

const API = import.meta.env.VITE_API_URL || '';

// ─── Icons ────────────────────────────────────────────────────────────────────
const Icon = ({ d, size = 20 }: { d: string; size?: number }) => (
    <svg width={size} height={size} viewBox="0 0 24 24" fill="none"
        stroke="currentColor" strokeWidth={1.8} strokeLinecap="round" strokeLinejoin="round">
        <path d={d} />
    </svg>
);

const icons = {
    close: "M18 6L6 18M6 6l12 12",
    refresh: "M23 4v6h-6M1 20v-6h6M3.51 9a9 9 0 0114.85-3.36L23 10M1 14l4.64 4.36A9 9 0 0020.49 15",
    nf: "M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8zM14 2v6h6M16 13H8M16 17H8M10 9H8",
    truck: "M1 3h15v13H1zM16 8h4l3 3v5h-7V8zM5.5 21a1.5 1.5 0 100-3 1.5 1.5 0 000 3zM18.5 21a1.5 1.5 0 100-3 1.5 1.5 0 000 3z",
    user: "M20 21v-2a4 4 0 00-4-4H8a4 4 0 00-4 4v2M12 11a4 4 0 100-8 4 4 0 000 8",
    map: "M3 9l9-7 9 7v11a2 2 0 01-2 2H5a2 2 0 01-2-2zM9 22V12h6v10",
    box: "M6 2L3 6v14a2 2 0 002 2h14a2 2 0 002-2V6l-3-4zM3 6h18M16 10a4 4 0 01-8 0",
    key: "M21 2l-2 2m-7.61 7.61a5.5 5.5 0 11-7.778 7.778 5.5 5.5 0 017.777-7.777zm0 0L15.5 7.5m0 0l3 3L22 7l-3-3m-3.5 3.5L19 4",
    check: "M20 6L9 17l-5-5",
};

// ─── Tipos ────────────────────────────────────────────────────────────────────
interface NotaFiscalDetalhe {
    cdNotaFiscal: number;
    cdSerNotaFiscal: string;
    noFilial?: string;
    noCliente?: string;
    cdCnpjCliente?: string;
    dtEmissaoNf?: string;
    icCancelado?: number;
    vlTransmissao?: number;
    cdAmbiente?: string;
    cdChaveNfe?: string;
    cdProtocolo?: string;
    cdModeloNf?: string;
}

interface FichaDetalhe {
    cdFicha: number;
    cdFilial: number;
    cdPlacaVeiculo?: string;
    cdTipoOperacao: number;
    cdPassoAtual: number;
    dtFicha?: string;
    dtSaida?: string;
    noMotorista?: string;
    cdFilialNavigation?: { noFilial?: string };
    cdPassoAtualNavigation?: { cdPasso: number; noPasso?: string };
    cdTipoOperacaoNavigation?: { noTipoOperacao?: string };
    notaFiscals?: NotaFiscalDetalhe[];
}

interface ModalFichaProps {
    cdFicha: number;
    onClose: () => void;
}

// ─── Passos fixos ─────────────────────────────────────────────────────────────
const PASSOS = [
    { cd: 1, nome: "Entrada" },
    { cd: 2, nome: "Pesagem" },
    { cd: 3, nome: "Carregamento" },
    { cd: 4, nome: "Saída" },
];

// ─── Spinner ──────────────────────────────────────────────────────────────────
function Spinner() {
    return (
        <div style={{
            display: "flex", alignItems: "center", justifyContent: "center",
            height: 200, flexDirection: "column", gap: 12
        }}>
            <div style={{
                width: 32, height: 32, borderRadius: "50%",
                border: "3px solid #1F2937", borderTopColor: "#E85D04",
                animation: "spin 0.8s linear infinite",
            }} />
            <style>{`@keyframes spin { to { transform: rotate(360deg); } }`}</style>
            <span style={{ fontSize: 13, color: "#6B7280" }}>Carregando ficha...</span>
        </div>
    );
}

// ─── Badge de Status NF ───────────────────────────────────────────────────────
function StatusNFBadge({ cancelado }: { cancelado?: number }) {
    const cancelada = cancelado === 1;
    return (
        <span style={{
            fontSize: 10, fontWeight: 700, padding: "2px 8px", borderRadius: 4,
            letterSpacing: "0.06em", textTransform: "uppercase",
            background: cancelada ? "rgba(239,68,68,0.12)" : "rgba(16,185,129,0.12)",
            color: cancelada ? "#EF4444" : "#10B981",
            border: `1px solid ${cancelada ? "#EF444440" : "#10B98140"}`,
        }}>
            {cancelada ? "Cancelada" : "Ativa"}
        </span>
    );
}

// ─── Campo de detalhe ─────────────────────────────────────────────────────────
function Campo({ label, value, mono = false }: { label: string; value?: string | null; mono?: boolean }) {
    return (
        <div>
            <div style={{
                fontSize: 10, color: "#6B7280", fontWeight: 700,
                textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 4
            }}>
                {label}
            </div>
            <div style={{
                fontSize: 13, color: value ? "#D1D5DB" : "#4B5563", fontWeight: 500,
                fontFamily: mono ? "'DM Mono', monospace" : "inherit",
            }}>
                {value || "—"}
            </div>
        </div>
    );
}

// ─── Timeline de Passos ───────────────────────────────────────────────────────
function TimelinePasso({ passoAtual }: { passoAtual: number }) {
    return (
        <div style={{ display: "flex", alignItems: "flex-start", gap: 0 }}>
            {PASSOS.map((p, i) => {
                const done = p.cd < passoAtual;
                const current = p.cd === passoAtual;
                return (
                    <div key={p.cd} style={{ display: "flex", alignItems: "center", flex: 1 }}>
                        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: 8 }}>
                            {/* Círculo */}
                            <div style={{
                                width: 36, height: 36, borderRadius: "50%",
                                background: done ? "#E85D04" : current ? "transparent" : "#1F2937",
                                border: current ? "2px solid #E85D04" : done ? "none" : "2px solid #374151",
                                display: "flex", alignItems: "center", justifyContent: "center",
                                fontSize: 12, fontWeight: 700,
                                color: done ? "white" : current ? "#E85D04" : "#6B7280",
                                boxShadow: current ? "0 0 16px #E85D0450" : "none",
                                transition: "all 0.3s", flexShrink: 0,
                            }}>
                                {done ? <Icon d={icons.check} size={15} /> : p.cd}
                            </div>
                            {/* Label */}
                            <span style={{
                                fontSize: 11, fontWeight: current ? 700 : 500, whiteSpace: "nowrap",
                                color: current ? "#F97316" : done ? "#D1D5DB" : "#4B5563",
                            }}>
                                {p.nome}
                            </span>
                        </div>
                        {/* Linha conectora */}
                        {i < PASSOS.length - 1 && (
                            <div style={{
                                flex: 1, height: 2, marginBottom: 24,
                                background: done ? "#E85D04" : "#1F2937",
                                transition: "background 0.3s",
                            }} />
                        )}
                    </div>
                );
            })}
        </div>
    );
}

// ─── Card de NF ───────────────────────────────────────────────────────────────
function CardNF({ nf }: { nf: NotaFiscalDetalhe }) {
    const [expanded, setExpanded] = useState(false);
    const prod = nf.cdAmbiente === "1";

    return (
        <div style={{
            background: "#0D1117", border: "1px solid #1F2937",
            borderRadius: 8, overflow: "hidden",
            transition: "border-color 0.15s",
        }}
            onMouseEnter={e => (e.currentTarget.style.borderColor = "#374151")}
            onMouseLeave={e => (e.currentTarget.style.borderColor = "#1F2937")}
        >
            {/* Header do card */}
            <div
                onClick={() => setExpanded(e => !e)}
                style={{
                    display: "flex", alignItems: "center", gap: 12,
                    padding: "12px 16px", cursor: "pointer",
                }}
            >
                <div style={{
                    width: 32, height: 32, borderRadius: 6, flexShrink: 0,
                    background: "rgba(59,130,246,0.1)", border: "1px solid rgba(59,130,246,0.2)",
                    display: "flex", alignItems: "center", justifyContent: "center",
                    color: "#3B82F6",
                }}>
                    <Icon d={icons.nf} size={15} />
                </div>

                <div style={{ flex: 1, minWidth: 0 }}>
                    <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
                        <span style={{
                            fontFamily: "'DM Mono', monospace", fontSize: 13,
                            color: "#3B82F6", fontWeight: 700
                        }}>
                            NF {nf.cdNotaFiscal}
                        </span>
                        <span style={{ fontSize: 11, color: "#4B5563" }}>/ {nf.cdSerNotaFiscal}</span>
                        <StatusNFBadge cancelado={nf.icCancelado} />
                        <span style={{
                            fontSize: 10, fontWeight: 700, padding: "2px 8px", borderRadius: 4,
                            letterSpacing: "0.06em", textTransform: "uppercase",
                            background: prod ? "rgba(59,130,246,0.12)" : "rgba(245,158,11,0.12)",
                            color: prod ? "#3B82F6" : "#F59E0B",
                            border: `1px solid ${prod ? "#3B82F640" : "#F59E0B40"}`,
                        }}>
                            {prod ? "Produção" : "Homolog"}
                        </span>
                    </div>
                    <div style={{ fontSize: 12, color: "#6B7280", marginTop: 2 }}>
                        {nf.noCliente ?? "—"} {nf.dtEmissaoNf ? `· ${new Date(nf.dtEmissaoNf).toLocaleDateString("pt-BR")}` : ""}
                    </div>
                </div>

                {nf.vlTransmissao != null && (
                    <span style={{
                        fontFamily: "'DM Mono', monospace", fontSize: 13,
                        color: "#10B981", fontWeight: 700, flexShrink: 0
                    }}>
                        R$ {nf.vlTransmissao.toFixed(2)}
                    </span>
                )}

                <div style={{
                    color: "#6B7280", fontSize: 12,
                    transform: expanded ? "rotate(90deg)" : "none",
                    transition: "transform 0.2s",
                }}>›</div>
            </div>

            {/* Detalhe expandido */}
            {expanded && (
                <div style={{ padding: "0 16px 16px", borderTop: "1px solid #1F2937" }}>
                    <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr 1fr", gap: 16, marginTop: 16 }}>
                        <Campo label="CNPJ Cliente" value={nf.cdCnpjCliente} mono />
                        <Campo label="Modelo NF" value={nf.cdModeloNf} />
                        <Campo label="Data Emissão"
                            value={nf.dtEmissaoNf ? new Date(nf.dtEmissaoNf).toLocaleDateString("pt-BR") : null} />
                    </div>

                    {nf.cdChaveNfe && (
                        <div style={{
                            marginTop: 14, padding: "10px 14px",
                            background: "#111827", borderRadius: 6, border: "1px solid #1F2937"
                        }}>
                            <div style={{
                                fontSize: 10, color: "#6B7280", fontWeight: 700,
                                textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 4
                            }}>
                                Chave NF-e
                            </div>
                            <div style={{
                                fontFamily: "'DM Mono', monospace", fontSize: 11,
                                color: "#3B82F6", wordBreak: "break-all"
                            }}>
                                {nf.cdChaveNfe}
                            </div>
                        </div>
                    )}

                    {nf.cdProtocolo && (
                        <div style={{
                            marginTop: 10, padding: "10px 14px",
                            background: "#111827", borderRadius: 6, border: "1px solid #1F2937"
                        }}>
                            <div style={{
                                fontSize: 10, color: "#6B7280", fontWeight: 700,
                                textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 4
                            }}>
                                Protocolo SEFAZ
                            </div>
                            <div style={{ fontFamily: "'DM Mono', monospace", fontSize: 12, color: "#D1D5DB" }}>
                                {nf.cdProtocolo}
                            </div>
                        </div>
                    )}
                </div>
            )}
        </div>
    );
}

// ─── Modal Principal ──────────────────────────────────────────────────────────
export default function ModalFicha({ cdFicha, onClose }: ModalFichaProps) {
    const [ficha, setFicha] = useState<FichaDetalhe | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const load = async () => {
        setLoading(true);
        setError(null);
        try {
            const res = await fetch(`${API}/api/Ficha/${cdFicha}`);
            if (!res.ok) throw new Error(`HTTP ${res.status}`);
            setFicha(await res.json());
        } catch (e: any) {
            setError(e.message ?? "Erro ao carregar ficha");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => { load(); }, [cdFicha]);

    // Fechar com ESC
    useEffect(() => {
        const handler = (e: KeyboardEvent) => { if (e.key === "Escape") onClose(); };
        window.addEventListener("keydown", handler);
        return () => window.removeEventListener("keydown", handler);
    }, [onClose]);

    const noFilial = ficha?.cdFilialNavigation?.noFilial ?? `Filial ${ficha?.cdFilial}`;
    const noTipoOp = ficha?.cdTipoOperacaoNavigation?.noTipoOperacao ?? `Op. ${ficha?.cdTipoOperacao}`;
    const passoAtual = ficha?.cdPassoAtualNavigation?.cdPasso ?? ficha?.cdPassoAtual ?? 1;
    const nomePassoAtual = ficha?.cdPassoAtualNavigation?.noPasso ?? "—";
    const nfs = ficha?.notaFiscals ?? [];

    return (
        <>
            {/* Overlay */}
            <div
                onClick={onClose}
                style={{
                    position: "fixed", inset: 0, background: "rgba(0,0,0,0.7)",
                    zIndex: 100, backdropFilter: "blur(2px)",
                    animation: "fadeIn 0.15s ease",
                }}
            />

            {/* Modal */}
            <div style={{
                position: "fixed", top: "50%", left: "50%",
                transform: "translate(-50%, -50%)",
                width: "min(780px, 95vw)", maxHeight: "90vh",
                background: "#111827", border: "1px solid #1F2937",
                borderRadius: 12, zIndex: 101, display: "flex", flexDirection: "column",
                animation: "slideUp 0.2s ease",
                overflow: "hidden",
            }}>
                <style>{`
          @keyframes fadeIn { from { opacity: 0 } to { opacity: 1 } }
          @keyframes slideUp { from { opacity: 0; transform: translate(-50%, -48%) } to { opacity: 1; transform: translate(-50%, -50%) } }
        `}</style>

                {/* Topo colorido */}
                <div style={{ height: 3, background: "linear-gradient(90deg, #E85D04, #9A3412, transparent)", flexShrink: 0 }} />

                {/* Header */}
                <div style={{
                    display: "flex", alignItems: "center", justifyContent: "space-between",
                    padding: "20px 24px 16px", flexShrink: 0,
                    borderBottom: "1px solid #1F2937",
                }}>
                    <div style={{ display: "flex", alignItems: "center", gap: 12 }}>
                        <div style={{ fontSize: 18, fontWeight: 800, color: "#F9FAFB" }}>
                            Ficha{" "}
                            <span style={{ color: "#E85D04", fontFamily: "'DM Mono', monospace" }}>
                                #{String(cdFicha).padStart(4, "0")}
                            </span>
                        </div>
                        {ficha && (
                            <span style={{
                                fontSize: 11, fontWeight: 700, padding: "3px 10px", borderRadius: 4,
                                background: "rgba(232,93,4,0.12)", color: "#F97316",
                                border: "1px solid rgba(232,93,4,0.25)",
                                textTransform: "uppercase", letterSpacing: "0.06em",
                            }}>
                                {nomePassoAtual}
                            </span>
                        )}
                    </div>
                    <div style={{ display: "flex", gap: 8 }}>
                        <button onClick={load} style={{
                            background: "#1F2937", border: "1px solid #374151", borderRadius: 6,
                            color: "#9CA3AF", cursor: "pointer", padding: "6px 10px",
                            display: "flex", alignItems: "center", gap: 5, fontSize: 12,
                        }}>
                            <Icon d={icons.refresh} size={13} />
                        </button>
                        <button onClick={onClose} style={{
                            background: "#1F2937", border: "1px solid #374151", borderRadius: 6,
                            color: "#9CA3AF", cursor: "pointer", padding: "6px 10px",
                            display: "flex", alignItems: "center",
                        }}>
                            <Icon d={icons.close} size={15} />
                        </button>
                    </div>
                </div>

                {/* Corpo com scroll */}
                <div style={{ flex: 1, overflowY: "auto", padding: "24px" }}>
                    {loading ? <Spinner /> : error ? (
                        <div style={{ textAlign: "center", padding: 40, color: "#EF4444" }}>
                            <div style={{ fontSize: 32, marginBottom: 8 }}>⚠️</div>
                            <div style={{ fontSize: 14, fontWeight: 600 }}>{error}</div>
                        </div>
                    ) : ficha ? (
                        <>
                            {/* ── Informações principais ── */}
                            <div style={{ marginBottom: 28 }}>
                                <div style={{
                                    fontSize: 11, fontWeight: 700, color: "#E85D04",
                                    textTransform: "uppercase", letterSpacing: "0.12em",
                                    marginBottom: 16, display: "flex", alignItems: "center", gap: 8
                                }}>
                                    <Icon d={icons.box} size={13} /> Dados da Operação
                                </div>
                                <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr 1fr", gap: 16 }}>
                                    <Campo label="Filial" value={noFilial} />
                                    <Campo label="Tipo de Operação" value={noTipoOp} />
                                    <Campo label="Data"
                                        value={ficha.dtFicha ? new Date(ficha.dtFicha).toLocaleDateString("pt-BR") : null} />
                                </div>
                            </div>

                            <div style={{ borderTop: "1px solid #1F2937", marginBottom: 28 }} />

                            {/* ── Veículo e motorista ── */}
                            <div style={{ marginBottom: 28 }}>
                                <div style={{
                                    fontSize: 11, fontWeight: 700, color: "#E85D04",
                                    textTransform: "uppercase", letterSpacing: "0.12em",
                                    marginBottom: 16, display: "flex", alignItems: "center", gap: 8
                                }}>
                                    <Icon d={icons.truck} size={13} /> Veículo e Motorista
                                </div>
                                <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr 1fr", gap: 16 }}>
                                    <Campo label="Placa" value={ficha.cdPlacaVeiculo} mono />
                                    <Campo label="Motorista" value={ficha.noMotorista} />
                                    <Campo label="Saída"
                                        value={ficha.dtSaida ? new Date(ficha.dtSaida).toLocaleDateString("pt-BR") : null} />
                                </div>
                            </div>

                            <div style={{ borderTop: "1px solid #1F2937", marginBottom: 28 }} />

                            {/* ── Timeline de passos ── */}
                            <div style={{ marginBottom: 28 }}>
                                <div style={{
                                    fontSize: 11, fontWeight: 700, color: "#E85D04",
                                    textTransform: "uppercase", letterSpacing: "0.12em",
                                    marginBottom: 20, display: "flex", alignItems: "center", gap: 8
                                }}>
                                    <Icon d={icons.map} size={13} /> Progresso da Operação
                                </div>
                                <TimelinePasso passoAtual={passoAtual} />
                            </div>

                            <div style={{ borderTop: "1px solid #1F2937", marginBottom: 28 }} />

                            {/* ── Notas Fiscais ── */}
                            <div>
                                <div style={{
                                    fontSize: 11, fontWeight: 700, color: "#3B82F6",
                                    textTransform: "uppercase", letterSpacing: "0.12em",
                                    marginBottom: 16, display: "flex", alignItems: "center",
                                    justifyContent: "space-between",
                                }}>
                                    <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
                                        <Icon d={icons.nf} size={13} />
                                        Notas Fiscais Vinculadas
                                    </div>
                                    <span style={{
                                        fontSize: 11, fontWeight: 700, padding: "2px 10px", borderRadius: 10,
                                        background: "rgba(59,130,246,0.12)", color: "#3B82F6",
                                        border: "1px solid rgba(59,130,246,0.2)",
                                    }}>
                                        {nfs.length} {nfs.length === 1 ? "nota" : "notas"}
                                    </span>
                                </div>

                                {nfs.length === 0 ? (
                                    <div style={{
                                        padding: "32px 20px", textAlign: "center",
                                        background: "#0D1117", borderRadius: 8,
                                        border: "1px dashed #1F2937",
                                    }}>
                                        <div style={{ fontSize: 24, marginBottom: 8 }}>📄</div>
                                        <div style={{ fontSize: 13, color: "#6B7280" }}>
                                            Nenhuma nota fiscal vinculada a esta ficha
                                        </div>
                                    </div>
                                ) : (
                                    <div style={{ display: "flex", flexDirection: "column", gap: 10 }}>
                                        {nfs.map(nf => (
                                            <CardNF key={`${nf.cdNotaFiscal}-${nf.cdSerNotaFiscal}`} nf={nf} />
                                        ))}
                                    </div>
                                )}
                            </div>
                        </>
                    ) : null}
                </div>
            </div>
        </>
    );
}
