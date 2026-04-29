import { useState, useEffect, useMemo, useCallback } from "react";
import { icons } from "./types";
import { Icon, Spinner, ErrorBox } from "./ui";

const API = import.meta.env.VITE_API_URL || '';

// ─── Tipos ────────────────────────────────────────────────────────────────────
interface Veiculo {
    placa: string;
    codigoFilial: number;
    ufVeiculo: string | null;
    codigoRntc: string | null;
}

interface Filial {
    codigo: number;
    nome: string | null;
}

// ─── Estilos ──────────────────────────────────────────────────────────────────
const thStyle: React.CSSProperties = {
    padding: "10px 16px", fontSize: 11, fontWeight: 700, color: "#6B7280",
    textTransform: "uppercase", letterSpacing: "0.08em", textAlign: "left",
    borderBottom: "1px solid #1F2937", background: "#0D1117",
};
const tdStyle: React.CSSProperties = {
    padding: "12px 16px", fontSize: 13, color: "#D1D5DB", whiteSpace: "nowrap",
};
const inputStyle: React.CSSProperties = {
    width: "100%", background: "#0D1117", border: "1px solid #374151",
    borderRadius: 6, color: "#F9FAFB", fontSize: 13, padding: "10px 14px",
    outline: "none", boxSizing: "border-box", fontFamily: "inherit",
};

// ─── Modal Cadastro ───────────────────────────────────────────────────────────
function ModalNovoVeiculo({ filiais, onSalvo, onFechar }: {
    filiais: Filial[];
    onSalvo: (v: Veiculo) => void;
    onFechar: () => void;
}) {
    const [placa, setPlaca] = useState("");
    const [codigoFilial, setCodigoFilial] = useState(filiais.length > 0 ? String(filiais[0].codigo) : "");
    const [uf, setUf] = useState("");
    const [rntc, setRntc] = useState("");
    const [loading, setLoading] = useState(false);
    const [erro, setErro] = useState<string | null>(null);

    const handleSalvar = async () => {
        const placaFmt = placa.toUpperCase().trim();
        if (!placaFmt || !codigoFilial) return;
        setLoading(true);
        setErro(null);
        try {
            const res = await fetch(`${API}/api/Veiculo`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    placa: placaFmt,
                    codigoFilial: Number(codigoFilial),
                    ufVeiculo: uf.trim() || null,
                    codigoRntc: rntc.trim() || null,
                }),
            });
            if (!res.ok) throw new Error(`HTTP ${res.status}`);
            onSalvo({ placa: placaFmt, codigoFilial: Number(codigoFilial), ufVeiculo: uf || null, codigoRntc: rntc || null });
        } catch (e: any) {
            setErro(e.message ?? "Erro ao cadastrar veículo");
        } finally {
            setLoading(false);
        }
    };

    return (
        <>
            <div onClick={onFechar} style={{ position: "fixed", inset: 0, background: "rgba(0,0,0,0.6)", zIndex: 100 }} />
            <div style={{
                position: "fixed", top: "50%", left: "50%", transform: "translate(-50%, -50%)",
                background: "#111827", border: "1px solid #374151", borderRadius: 10,
                padding: 28, width: 400, zIndex: 101, boxShadow: "0 20px 60px rgba(0,0,0,0.5)",
            }}>
                <div style={{ height: 3, background: "linear-gradient(90deg, #E85D04, #9A3412, transparent)", margin: "-28px -28px 24px" }} />
                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 20 }}>
                    <div style={{ fontSize: 15, fontWeight: 700, color: "#F9FAFB" }}>Cadastrar Veículo</div>
                    <button onClick={onFechar} style={{ background: "none", border: "none", color: "#6B7280", cursor: "pointer", fontSize: 18 }}>✕</button>
                </div>

                <div style={{ display: "flex", flexDirection: "column", gap: 14 }}>
                    <div>
                        <label style={{ fontSize: 11, fontWeight: 700, color: "#6B7280", textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 6, display: "block" }}>
                            Placa *
                        </label>
                        <input value={placa} onChange={e => setPlaca(e.target.value.toUpperCase())}
                            placeholder="ABC-1234" maxLength={8} autoFocus
                            style={{ ...inputStyle, fontFamily: "'DM Mono', monospace", letterSpacing: "0.1em" }} />
                    </div>
                    <div>
                        <label style={{ fontSize: 11, fontWeight: 700, color: "#6B7280", textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 6, display: "block" }}>
                            Filial *
                        </label>
                        <select value={codigoFilial} onChange={e => setCodigoFilial(e.target.value)}
                            style={{ ...inputStyle, cursor: "pointer" }}>
                            {filiais.map(f => (
                                <option key={f.codigo} value={String(f.codigo)}>
                                    {f.nome ?? `Filial ${f.codigo}`}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 12 }}>
                        <div>
                            <label style={{ fontSize: 11, fontWeight: 700, color: "#6B7280", textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 6, display: "block" }}>
                                UF
                            </label>
                            <input value={uf} onChange={e => setUf(e.target.value.toUpperCase())}
                                placeholder="SP" maxLength={2} style={inputStyle} />
                        </div>
                        <div>
                            <label style={{ fontSize: 11, fontWeight: 700, color: "#6B7280", textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 6, display: "block" }}>
                                RNTC
                            </label>
                            <input value={rntc} onChange={e => setRntc(e.target.value)}
                                placeholder="Código RNTC" style={inputStyle} />
                        </div>
                    </div>
                </div>

                {erro && <div style={{ marginTop: 12, fontSize: 12, color: "#EF4444" }}>⚠️ {erro}</div>}

                <div style={{ display: "flex", gap: 8, justifyContent: "flex-end", marginTop: 20 }}>
                    <button onClick={onFechar} style={{
                        background: "transparent", border: "1px solid #374151", borderRadius: 6,
                        color: "#9CA3AF", fontSize: 12, fontWeight: 600, padding: "8px 16px", cursor: "pointer",
                    }}>Cancelar</button>
                    <button onClick={handleSalvar} disabled={!placa.trim() || loading} style={{
                        background: !placa.trim() || loading ? "#374151" : "linear-gradient(135deg, #E85D04, #9A3412)",
                        border: "none", borderRadius: 6, color: "white", fontSize: 12, fontWeight: 700,
                        padding: "8px 16px", cursor: !placa.trim() || loading ? "not-allowed" : "pointer",
                        display: "flex", alignItems: "center", gap: 6,
                    }}>
                        {loading ? <><Icon d={icons.refresh} size={12} /> Salvando...</> : "✓ Salvar"}
                    </button>
                </div>
            </div>
        </>
    );
}

// ─── Modal Editar ─────────────────────────────────────────────────────────────
function ModalEditarVeiculo({ veiculo, filiais, onEditado, onFechar }: {
    veiculo: Veiculo;
    filiais: Filial[];
    onEditado: (v: Veiculo) => void;
    onFechar: () => void;
}) {
    const [codigoFilial, setCodigoFilial] = useState(String(veiculo.codigoFilial));
    const [uf, setUf] = useState(veiculo.ufVeiculo ?? "");
    const [rntc, setRntc] = useState(veiculo.codigoRntc ?? "");
    const [loading, setLoading] = useState(false);
    const [erro, setErro] = useState<string | null>(null);

    const handleSalvar = async () => {
        setLoading(true);
        setErro(null);
        try {
            const res = await fetch(`${API}/api/Veiculo/${veiculo.placa}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    placa: veiculo.placa,
                    codigoFilial: Number(codigoFilial),
                    ufVeiculo: uf.trim() || null,
                    codigoRntc: rntc.trim() || null,
                }),
            });
            if (!res.ok) throw new Error(`HTTP ${res.status}`);
            onEditado({ placa: veiculo.placa, codigoFilial: Number(codigoFilial), ufVeiculo: uf || null, codigoRntc: rntc || null });
        } catch (e: any) {
            setErro(e.message ?? "Erro ao editar veículo");
        } finally {
            setLoading(false);
        }
    };

    return (
        <>
            <div onClick={onFechar} style={{ position: "fixed", inset: 0, background: "rgba(0,0,0,0.6)", zIndex: 100 }} />
            <div style={{
                position: "fixed", top: "50%", left: "50%", transform: "translate(-50%, -50%)",
                background: "#111827", border: "1px solid #374151", borderRadius: 10,
                padding: 28, width: 400, zIndex: 101, boxShadow: "0 20px 60px rgba(0,0,0,0.5)",
            }}>
                <div style={{ height: 3, background: "linear-gradient(90deg, #3B82F6, #1D4ED8, transparent)", margin: "-28px -28px 24px" }} />
                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 20 }}>
                    <div style={{ fontSize: 15, fontWeight: 700, color: "#F9FAFB" }}>
                        Editar Veículo <span style={{ color: "#3B82F6", fontFamily: "'DM Mono', monospace" }}>{veiculo.placa}</span>
                    </div>
                    <button onClick={onFechar} style={{ background: "none", border: "none", color: "#6B7280", cursor: "pointer", fontSize: 18 }}>✕</button>
                </div>

                <div style={{ display: "flex", flexDirection: "column", gap: 14 }}>
                    <div>
                        <label style={{ fontSize: 11, fontWeight: 700, color: "#6B7280", textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 6, display: "block" }}>
                            Placa (somente leitura)
                        </label>
                        <input value={veiculo.placa} disabled
                            style={{ ...inputStyle, opacity: 0.5, cursor: "not-allowed", fontFamily: "'DM Mono', monospace", letterSpacing: "0.1em" }} />
                    </div>
                    <div>
                        <label style={{ fontSize: 11, fontWeight: 700, color: "#6B7280", textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 6, display: "block" }}>
                            Filial *
                        </label>
                        <select value={codigoFilial} onChange={e => setCodigoFilial(e.target.value)}
                            style={{ ...inputStyle, cursor: "pointer" }}>
                            {filiais.map(f => (
                                <option key={f.codigo} value={String(f.codigo)}>
                                    {f.nome ?? `Filial ${f.codigo}`}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 12 }}>
                        <div>
                            <label style={{ fontSize: 11, fontWeight: 700, color: "#6B7280", textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 6, display: "block" }}>
                                UF
                            </label>
                            <input value={uf} onChange={e => setUf(e.target.value.toUpperCase())}
                                placeholder="SP" maxLength={2} style={inputStyle} />
                        </div>
                        <div>
                            <label style={{ fontSize: 11, fontWeight: 700, color: "#6B7280", textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 6, display: "block" }}>
                                RNTC
                            </label>
                            <input value={rntc} onChange={e => setRntc(e.target.value)}
                                placeholder="Código RNTC" style={inputStyle} />
                        </div>
                    </div>
                </div>

                {erro && <div style={{ marginTop: 12, fontSize: 12, color: "#EF4444" }}>⚠️ {erro}</div>}

                <div style={{ display: "flex", gap: 8, justifyContent: "flex-end", marginTop: 20 }}>
                    <button onClick={onFechar} style={{
                        background: "transparent", border: "1px solid #374151", borderRadius: 6,
                        color: "#9CA3AF", fontSize: 12, fontWeight: 600, padding: "8px 16px", cursor: "pointer",
                    }}>Cancelar</button>
                    <button onClick={handleSalvar} disabled={loading} style={{
                        background: loading ? "#374151" : "linear-gradient(135deg, #3B82F6, #1D4ED8)",
                        border: "none", borderRadius: 6, color: "white", fontSize: 12, fontWeight: 700,
                        padding: "8px 16px", cursor: loading ? "not-allowed" : "pointer",
                        display: "flex", alignItems: "center", gap: 6,
                    }}>
                        {loading ? <><Icon d={icons.refresh} size={12} /> Salvando...</> : "✎ Salvar alterações"}
                    </button>
                </div>
            </div>
        </>
    );
}

// ─── Página principal ─────────────────────────────────────────────────────────
export default function VeiculosPage() {
    const [veiculos, setVeiculos] = useState<Veiculo[]>([]);
    const [filiais, setFiliais] = useState<Filial[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [search, setSearch] = useState("");
    const [modalAberto, setModalAberto] = useState(false);
    const [modalEditarAberto, setModalEditarAberto] = useState(false);
    const [selected, setSelected] = useState<Veiculo | null>(null);

    const load = useCallback(async () => {
        setLoading(true);
        setError(null);
        try {
            const [resV, resF] = await Promise.all([
                fetch(`${API}/api/Veiculo`),
                fetch(`${API}/api/Filial`),
            ]);
            if (!resV.ok) throw new Error(`HTTP ${resV.status}`);
            if (!resF.ok) throw new Error(`HTTP ${resF.status}`);
            setVeiculos(await resV.json());
            setFiliais(await resF.json());
        } catch (e: any) {
            setError(e.message ?? "Erro ao carregar veículos");
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => { load(); }, [load]);

    const filtrados = useMemo(() =>
        veiculos.filter(v =>
            v.placa.toLowerCase().includes(search.toLowerCase()) ||
            String(v.codigoFilial).includes(search) ||
            (v.ufVeiculo ?? "").toLowerCase().includes(search.toLowerCase())
        ), [veiculos, search]);

    const handleSalvo = useCallback((novo: Veiculo) => {
        setVeiculos(v => [...v, novo]);
        setModalAberto(false);
    }, []);

    const handleEditado = useCallback((atualizado: Veiculo) => {
        setVeiculos(v => v.map(x => x.placa === atualizado.placa ? atualizado : x));
        setSelected(atualizado);
        setModalEditarAberto(false);
    }, []);

    return (
        <div>
            {/* Header */}
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start", marginBottom: 24 }}>
                <div>
                    <h1 style={{ fontSize: 22, fontWeight: 800, color: "#F9FAFB", margin: 0, letterSpacing: "-0.03em" }}>Veículos</h1>
                    <p style={{ fontSize: 13, color: "#6B7280", margin: "4px 0 0" }}>
                        {loading ? "Carregando..." : `${veiculos.length} veículos cadastrados`}
                    </p>
                </div>
                <div style={{ display: "flex", gap: 8 }}>
                    <button onClick={load} style={{
                        background: "#1F2937", border: "1px solid #374151", borderRadius: 6,
                        color: "#9CA3AF", fontSize: 12, padding: "9px 14px",
                        cursor: "pointer", display: "flex", alignItems: "center", gap: 6,
                    }}>
                        <Icon d={icons.refresh} size={13} /> Atualizar
                    </button>
                    <button onClick={() => setModalAberto(true)} style={{
                        background: "linear-gradient(135deg, #E85D04, #9A3412)",
                        border: "none", borderRadius: 6, color: "white",
                        fontWeight: 700, fontSize: 13, padding: "9px 18px", cursor: "pointer",
                    }}>
                        + Novo Veículo
                    </button>
                </div>
            </div>

            {/* KPIs */}
            <div style={{ display: "grid", gridTemplateColumns: "repeat(3, 1fr)", gap: 16, marginBottom: 20 }}>
                {[
                    { label: "Total", valor: veiculos.length, cor: "#E85D04" },
                    { label: "Com UF", valor: veiculos.filter(v => v.ufVeiculo).length, cor: "#10B981" },
                    { label: "Com RNTC", valor: veiculos.filter(v => v.codigoRntc).length, cor: "#3B82F6" },
                ].map(k => (
                    <div key={k.label} style={{
                        background: "#111827", borderRadius: 8, padding: "16px 20px",
                        border: "1px solid #1F2937", borderLeftWidth: 4, borderLeftColor: k.cor,
                        borderLeftStyle: "solid",
                    }}>
                        <div style={{ fontSize: 11, color: "#6B7280", fontWeight: 700, textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 6 }}>{k.label}</div>
                        <div style={{ fontSize: 32, fontWeight: 800, color: k.cor }}>{k.valor}</div>
                    </div>
                ))}
            </div>

            {/* Busca */}
            <div style={{ marginBottom: 16, maxWidth: 320 }}>
                <input value={search} onChange={e => setSearch(e.target.value)}
                    placeholder="Buscar por placa, filial ou UF..."
                    style={{
                        width: "100%", background: "#111827", border: "1px solid #374151",
                        borderRadius: 6, color: "#D1D5DB", fontSize: 13, padding: "9px 14px",
                        outline: "none", boxSizing: "border-box",
                    }} />
            </div>

            {/* Tabela */}
            {loading ? <Spinner /> : error ? <ErrorBox message={error} onRetry={load} /> : (
                <div style={{ background: "#111827", border: "1px solid #1F2937", borderRadius: 8, overflow: "hidden" }}>
                    <table style={{ width: "100%", borderCollapse: "collapse" }}>
                        <thead>
                            <tr>
                                {["Placa", "Filial", "UF", "RNTC"].map(h => (
                                    <th key={h} style={thStyle}>{h}</th>
                                ))}
                            </tr>
                        </thead>
                        <tbody>
                            {filtrados.map(v => (
                                <tr key={v.placa}
                                    onClick={() => setSelected(s => s?.placa === v.placa ? null : v)}
                                    style={{
                                        cursor: "pointer",
                                        background: selected?.placa === v.placa ? "rgba(232,93,4,0.08)" : "transparent",
                                        borderBottom: "1px solid #1F2937", transition: "background 0.15s",
                                    }}
                                    onMouseEnter={e => { if (selected?.placa !== v.placa) e.currentTarget.style.background = "rgba(255,255,255,0.03)"; }}
                                    onMouseLeave={e => { if (selected?.placa !== v.placa) e.currentTarget.style.background = "transparent"; }}
                                >
                                    <td style={tdStyle}>
                                        <span style={{ fontFamily: "'DM Mono', monospace", fontSize: 13, color: "#E85D04", fontWeight: 700, letterSpacing: "0.08em" }}>
                                            {v.placa}
                                        </span>
                                    </td>
                                    <td style={tdStyle}>Filial {v.codigoFilial}</td>
                                    <td style={tdStyle}>
                                        {v.ufVeiculo
                                            ? <span style={{ background: "rgba(59,130,246,0.12)", color: "#3B82F6", border: "1px solid #3B82F640", borderRadius: 4, padding: "2px 8px", fontSize: 11, fontWeight: 700 }}>{v.ufVeiculo}</span>
                                            : <span style={{ color: "#4B5563" }}>—</span>
                                        }
                                    </td>
                                    <td style={tdStyle}>
                                        {v.codigoRntc
                                            ? <span style={{ fontFamily: "'DM Mono', monospace", fontSize: 12, color: "#9CA3AF" }}>{v.codigoRntc}</span>
                                            : <span style={{ color: "#4B5563" }}>—</span>
                                        }
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                    {filtrados.length === 0 && (
                        <div style={{ padding: 40, textAlign: "center", color: "#6B7280", fontSize: 13 }}>
                            Nenhum veículo encontrado.
                        </div>
                    )}
                </div>
            )}

            {/* Detalhe */}
            {selected && (
                <div style={{ marginTop: 20, background: "#111827", border: "1px solid #E85D0440", borderRadius: 8, padding: 24 }}>
                    <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 16 }}>
                        <div style={{ fontSize: 15, fontWeight: 800, color: "#F9FAFB" }}>
                            Veículo <span style={{ color: "#E85D04", fontFamily: "'DM Mono', monospace" }}>{selected.placa}</span>
                        </div>
                        <div style={{ display: "flex", gap: 8, alignItems: "center" }}>
                            <button onClick={() => setModalEditarAberto(true)} style={{
                                background: "linear-gradient(135deg, #3B82F6, #1D4ED8)",
                                border: "none", borderRadius: 6, color: "white",
                                fontSize: 12, fontWeight: 700, padding: "7px 14px", cursor: "pointer",
                            }}>✎ Editar</button>
                            <button onClick={() => setSelected(null)} style={{ background: "none", border: "none", color: "#6B7280", cursor: "pointer", fontSize: 18 }}>✕</button>
                        </div>
                    </div>
                    <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr 1fr 1fr", gap: 16 }}>
                        {[
                            ["Placa", selected.placa],
                            ["Filial", `Filial ${selected.codigoFilial}`],
                            ["UF", selected.ufVeiculo ?? "—"],
                            ["RNTC", selected.codigoRntc ?? "—"],
                        ].map(([k, v]) => (
                            <div key={k}>
                                <div style={{ fontSize: 10, color: "#6B7280", fontWeight: 700, textTransform: "uppercase", letterSpacing: "0.08em", marginBottom: 4 }}>{k}</div>
                                <div style={{ fontSize: 13, color: "#D1D5DB", fontWeight: 500, fontFamily: k === "Placa" || k === "RNTC" ? "'DM Mono', monospace" : "inherit" }}>{v}</div>
                            </div>
                        ))}
                    </div>
                </div>
            )}

            {/* Modais */}
            {modalAberto && <ModalNovoVeiculo filiais={filiais} onSalvo={handleSalvo} onFechar={() => setModalAberto(false)} />}
            {modalEditarAberto && selected && (
                <ModalEditarVeiculo
                    veiculo={selected}
                    filiais={filiais}
                    onEditado={handleEditado}
                    onFechar={() => setModalEditarAberto(false)}
                />
            )}
        </div>
    );
}
