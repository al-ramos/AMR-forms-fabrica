import { useState, useEffect } from "react";

const API = "http://localhost:5186";

// ─── Icons ────────────────────────────────────────────────────────────────────
const Icon = ({ d, size = 20 }: { d: string; size?: number }) => (
    <svg width={size} height={size} viewBox="0 0 24 24" fill="none"
        stroke="currentColor" strokeWidth={1.8} strokeLinecap="round" strokeLinejoin="round">
        <path d={d} />
    </svg>
);

const icons = {
    check: "M20 6L9 17l-5-5",
    close: "M18 6L6 18M6 6l12 12",
    loader: "M12 2v4M12 18v4M4.93 4.93l2.83 2.83M16.24 16.24l2.83 2.83M2 12h4M18 12h4M4.93 19.07l2.83-2.83M16.24 7.76l2.83-2.83",
    ficha: "M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2",
    user: "M20 21v-2a4 4 0 00-4-4H8a4 4 0 00-4 4v2M12 11a4 4 0 100-8 4 4 0 000 8",
    truck: "M1 3h15v13H1zM16 8h4l3 3v5h-7V8zM5.5 21a1.5 1.5 0 100-3 1.5 1.5 0 000 3zM18.5 21a1.5 1.5 0 100-3 1.5 1.5 0 000 3z",
    box: "M6 2L3 6v14a2 2 0 002 2h14a2 2 0 002-2V6l-3-4zM3 6h18M16 10a4 4 0 01-8 0",
    map: "M3 9l9-7 9 7v11a2 2 0 01-2 2H5a2 2 0 01-2-2zM9 22V12h6v10",
    note: "M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8zM14 2v6h6M16 13H8M16 17H8M10 9H8",
    arrow: "M19 12H5M12 5l-7 7 7 7",
};

// ─── Tipos ────────────────────────────────────────────────────────────────────
interface NovaFichaForm {
    codigoFilial: string;
    codigoTipoOperacao: string;
    placaVeiculo: string;
}

interface Veiculo {
    placa: string;
    codigoFilial: number;
}

interface NovaFichaProps {
    onBack: () => void;
    onSuccess: () => void;
}

// ─── Dados de seleção (mockados — troque por API se tiver endpoint de filiais) ─
const filiais = [
    { value: "1", label: "Filial São Paulo" },
    { value: "2", label: "Filial Campinas" },
    { value: "3", label: "Filial Ribeirão Preto" },
];

const tiposOperacao = [
    { value: "1", label: "Carga" },
    { value: "2", label: "Descarga" },
    { value: "3", label: "Transferência" },
];

// ─── Estilos base ─────────────────────────────────────────────────────────────
const inputStyle: React.CSSProperties = {
    width: "100%",
    background: "#0D1117",
    border: "1px solid #374151",
    borderRadius: 6,
    color: "#F9FAFB",
    fontSize: 13,
    padding: "10px 14px",
    outline: "none",
    boxSizing: "border-box",
    transition: "border-color 0.15s",
    fontFamily: "inherit",
};

const labelStyle: React.CSSProperties = {
    fontSize: 11,
    fontWeight: 700,
    color: "#6B7280",
    textTransform: "uppercase",
    letterSpacing: "0.08em",
    marginBottom: 6,
    display: "flex",
    alignItems: "center",
    gap: 6,
};

// ─── Campo individual ─────────────────────────────────────────────────────────
function Field({ label, icon, required, children }: {
    label: string; icon?: string; required?: boolean; children: React.ReactNode;
}) {
    return (
        <div style={{ display: "flex", flexDirection: "column" }}>
            <label style={labelStyle}>
                {icon && <Icon d={icon} size={12} />}
                {label}
                {required && <span style={{ color: "#E85D04", marginLeft: 2 }}>*</span>}
            </label>
            {children}
        </div>
    );
}

// ─── Componente principal ─────────────────────────────────────────────────────
export default function NovaFicha({ onBack, onSuccess }: NovaFichaProps) {
    const [form, setForm] = useState<NovaFichaForm>({
        codigoFilial: "",
        codigoTipoOperacao: "",
        placaVeiculo: "",
    });

    const [veiculos, setVeiculos] = useState<Veiculo[]>([]);
    const [loadingVeiculos, setLoadingVeiculos] = useState(false);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState(false);
    const [touched, setTouched] = useState<Set<string>>(new Set());

    // ── Carrega veículos ao mudar filial ──────────────────────────────────────
    useEffect(() => {
        if (!form.codigoFilial) {
            setVeiculos([]);
            setForm(f => ({ ...f, placaVeiculo: "" }));
            return;
        }
        setLoadingVeiculos(true);
        setForm(f => ({ ...f, placaVeiculo: "" }));
        fetch(`${API}/api/Veiculo/filial/${form.codigoFilial}`)
            .then(r => r.json())
            .then((data: Veiculo[]) => setVeiculos(data))
            .catch(() => setVeiculos([]))
            .finally(() => setLoadingVeiculos(false));
    }, [form.codigoFilial]);

    const set = (field: keyof NovaFichaForm) => (
        e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
    ) => {
        setForm(f => ({ ...f, [field]: e.target.value }));
        setTouched(t => new Set(t).add(field));
        setError(null);
    };

    const isValid = (field: keyof NovaFichaForm) =>
        !touched.has(field) || form[field].trim() !== "";

    const canSubmit = form.codigoFilial && form.codigoTipoOperacao && form.placaVeiculo;

    const handleSubmit = async () => {
        setTouched(new Set(["codigoFilial", "codigoTipoOperacao", "placaVeiculo"]));
        if (!canSubmit) return;
        setLoading(true);
        setError(null);

        try {
            // Payload exato conforme AbrirFichaCommand
            const payload = {
                codigoFilial: Number(form.codigoFilial),
                placaVeiculo: form.placaVeiculo,
                codigoTipoOperacao: Number(form.codigoTipoOperacao),
            };

            const res = await fetch(`${API}/api/Ficha`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload),
            });

            if (!res.ok) {
                const msg = await res.text().catch(() => `HTTP ${res.status}`);
                throw new Error(msg || `HTTP ${res.status}`);
            }

            setSuccess(true);
            setTimeout(() => onSuccess(), 1500);
        } catch (e: any) {
            setError(e.message ?? "Erro ao criar ficha");
        } finally {
            setLoading(false);
        }
    };

    // ── Tela de sucesso ───────────────────────────────────────────────────────
    if (success) {
        return (
            <div style={{
                display: "flex", flexDirection: "column", alignItems: "center",
                justifyContent: "center", height: 400, gap: 16
            }}>
                <div style={{
                    width: 64, height: 64, borderRadius: "50%",
                    background: "rgba(16,185,129,0.12)", border: "2px solid #10B981",
                    display: "flex", alignItems: "center", justifyContent: "center", color: "#10B981",
                }}>
                    <Icon d={icons.check} size={28} />
                </div>
                <div style={{ fontSize: 18, fontWeight: 800, color: "#F9FAFB" }}>Ficha criada com sucesso!</div>
                <div style={{ fontSize: 13, color: "#6B7280" }}>Redirecionando para a listagem...</div>
            </div>
        );
    }

    // ── Formulário ────────────────────────────────────────────────────────────
    return (
        <div>
            <div style={{ display: "flex", alignItems: "center", gap: 16, marginBottom: 28 }}>
                <button onClick={onBack} style={{
                    background: "#111827", border: "1px solid #1F2937", borderRadius: 6,
                    color: "#9CA3AF", cursor: "pointer", padding: "7px 12px",
                    display: "flex", alignItems: "center", gap: 6, fontSize: 12,
                }}>
                    <Icon d={icons.arrow} size={14} /> Voltar
                </button>
                <div>
                    <h1 style={{ fontSize: 22, fontWeight: 800, color: "#F9FAFB", margin: 0, letterSpacing: "-0.03em" }}>
                        Nova Ficha de Operação
                    </h1>
                    <p style={{ fontSize: 13, color: "#6B7280", margin: "4px 0 0" }}>
                        Preencha os dados para iniciar uma nova operação
                    </p>
                </div>
            </div>

            <div style={{
                background: "#111827", border: "1px solid #1F2937",
                borderRadius: 10, overflow: "hidden", maxWidth: 720
            }}>
                <div style={{ height: 3, background: "linear-gradient(90deg, #E85D04, #9A3412, transparent)" }} />

                <div style={{ padding: 32 }}>

                    {/* Seção 1 — Operação */}
                    <div style={{ marginBottom: 28 }}>
                        <div style={{
                            fontSize: 11, fontWeight: 700, color: "#E85D04",
                            textTransform: "uppercase", letterSpacing: "0.12em",
                            marginBottom: 18, display: "flex", alignItems: "center", gap: 8
                        }}>
                            <Icon d={icons.ficha} size={13} /> Dados da Operação
                        </div>

                        <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 16 }}>
                            <Field label="Filial" icon={icons.map} required>
                                <select value={form.codigoFilial} onChange={set("codigoFilial")}
                                    style={{ ...inputStyle, borderColor: !isValid("codigoFilial") ? "#EF4444" : "#374151", cursor: "pointer" }}>
                                    <option value="">Selecione a filial...</option>
                                    {filiais.map(f => <option key={f.value} value={f.value}>{f.label}</option>)}
                                </select>
                                {!isValid("codigoFilial") && <span style={{ fontSize: 11, color: "#EF4444", marginTop: 4 }}>Campo obrigatório</span>}
                            </Field>

                            <Field label="Tipo de Operação" icon={icons.box} required>
                                <select value={form.codigoTipoOperacao} onChange={set("codigoTipoOperacao")}
                                    style={{ ...inputStyle, borderColor: !isValid("codigoTipoOperacao") ? "#EF4444" : "#374151", cursor: "pointer" }}>
                                    <option value="">Selecione o tipo...</option>
                                    {tiposOperacao.map(t => <option key={t.value} value={t.value}>{t.label}</option>)}
                                </select>
                                {!isValid("codigoTipoOperacao") && <span style={{ fontSize: 11, color: "#EF4444", marginTop: 4 }}>Campo obrigatório</span>}
                            </Field>
                        </div>
                    </div>

                    <div style={{ borderTop: "1px solid #1F2937", marginBottom: 28 }} />

                    {/* Seção 2 — Veículo */}
                    <div style={{ marginBottom: 28 }}>
                        <div style={{
                            fontSize: 11, fontWeight: 700, color: "#E85D04",
                            textTransform: "uppercase", letterSpacing: "0.12em",
                            marginBottom: 18, display: "flex", alignItems: "center", gap: 8
                        }}>
                            <Icon d={icons.truck} size={13} /> Veículo
                        </div>

                        <Field label="Placa do Veículo" icon={icons.truck} required>
                            <select
                                value={form.placaVeiculo}
                                onChange={set("placaVeiculo")}
                                disabled={!form.codigoFilial || loadingVeiculos}
                                style={{
                                    ...inputStyle,
                                    borderColor: !isValid("placaVeiculo") ? "#EF4444" : "#374151",
                                    cursor: !form.codigoFilial ? "not-allowed" : "pointer",
                                    opacity: !form.codigoFilial ? 0.5 : 1,
                                    fontFamily: "'DM Mono', monospace",
                                    letterSpacing: "0.1em",
                                    maxWidth: 320,
                                }}
                            >
                                {!form.codigoFilial && <option value="">Selecione a filial primeiro...</option>}
                                {form.codigoFilial && loadingVeiculos && <option value="">Carregando veículos...</option>}
                                {form.codigoFilial && !loadingVeiculos && veiculos.length === 0 && (
                                    <option value="">Nenhum veículo cadastrado</option>
                                )}
                                {form.codigoFilial && !loadingVeiculos && veiculos.length > 0 && (
                                    <>
                                        <option value="">Selecione a placa...</option>
                                        {veiculos.map(v => (
                                            <option key={v.placa} value={v.placa}>{v.placa}</option>
                                        ))}
                                    </>
                                )}
                            </select>
                            {!isValid("placaVeiculo") && (
                                <span style={{ fontSize: 11, color: "#EF4444", marginTop: 4 }}>Campo obrigatório</span>
                            )}
                            {form.codigoFilial && !loadingVeiculos && veiculos.length === 0 && (
                                <span style={{ fontSize: 11, color: "#F59E0B", marginTop: 4 }}>
                                    ⚠️ Cadastre veículos para esta filial antes de criar a ficha
                                </span>
                            )}
                        </Field>
                    </div>

                    {/* Erro da API */}
                    {error && (
                        <div style={{
                            marginBottom: 20, padding: "12px 16px",
                            background: "rgba(239,68,68,0.08)", border: "1px solid rgba(239,68,68,0.3)",
                            borderRadius: 6, display: "flex", alignItems: "center", gap: 10,
                        }}>
                            <Icon d={icons.close} size={16} />
                            <div>
                                <div style={{ fontSize: 13, fontWeight: 600, color: "#EF4444" }}>Erro ao criar ficha</div>
                                <div style={{ fontSize: 12, color: "#FCA5A5", marginTop: 2 }}>{error}</div>
                            </div>
                        </div>
                    )}

                    {/* Ações */}
                    <div style={{ display: "flex", gap: 10, justifyContent: "flex-end" }}>
                        <button onClick={onBack} disabled={loading} style={{
                            background: "transparent", border: "1px solid #374151", borderRadius: 6,
                            color: "#9CA3AF", fontSize: 13, fontWeight: 600, padding: "10px 20px",
                            cursor: loading ? "not-allowed" : "pointer", opacity: loading ? 0.5 : 1,
                        }}>
                            Cancelar
                        </button>

                        <button onClick={handleSubmit} disabled={loading || !canSubmit} style={{
                            background: loading || !canSubmit ? "#374151" : "linear-gradient(135deg, #E85D04, #9A3412)",
                            border: "none", borderRadius: 6, color: "white", fontSize: 13, fontWeight: 700,
                            padding: "10px 24px", cursor: loading || !canSubmit ? "not-allowed" : "pointer",
                            display: "flex", alignItems: "center", gap: 8,
                            transition: "all 0.15s", opacity: !canSubmit ? 0.6 : 1,
                        }}>
                            {loading
                                ? <><Icon d={icons.loader} size={14} /> Criando ficha...</>
                                : <><Icon d={icons.check} size={14} /> Criar Ficha</>
                            }
                        </button>
                    </div>

                </div>
            </div>
        </div>
    );
}
