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
    cdFilial: string;
    cdTipoOperacao: string;
    cdPlacaVeiculo: string;
    noMotorista: string;
    cdPedido: string;
    noObservacao: string;
}

interface Veiculo {
    cdPlacaVeiculo: string;
    cdFilial: number;
}

interface NovaFichaProps {
    onBack: () => void;
    onSuccess: () => void;
}

// ─── Dados de seleção ─────────────────────────────────────────────────────────
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
function Field({
    label, icon, required, children,
}: {
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
        cdFilial: "",
        cdTipoOperacao: "",
        cdPlacaVeiculo: "",
        noMotorista: "",
        cdPedido: "",
        noObservacao: "",
    });

    const [veiculos, setVeiculos] = useState<Veiculo[]>([]);
    const [loadingVeiculos, setLoadingVeiculos] = useState(false);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState(false);
    const [touched, setTouched] = useState<Set<string>>(new Set());

    // ── Carrega veículos ao mudar filial ──────────────────────────────────────
    useEffect(() => {
        if (!form.cdFilial) {
            setVeiculos([]);
            setForm(f => ({ ...f, cdPlacaVeiculo: "" }));
            return;
        }
        setLoadingVeiculos(true);
        setForm(f => ({ ...f, cdPlacaVeiculo: "" }));
        fetch(`${API}/api/Veiculo/filial/${form.cdFilial}`)
            .then(r => r.json())
            .then(data => setVeiculos(data))
            .catch(() => setVeiculos([]))
            .finally(() => setLoadingVeiculos(false));
    }, [form.cdFilial]);

    const set = (field: keyof NovaFichaForm) => (
        e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>
    ) => {
        setForm(f => ({ ...f, [field]: e.target.value }));
        setTouched(t => new Set(t).add(field));
        setError(null);
    };

    const isValid = (field: keyof NovaFichaForm) =>
        !touched.has(field) || form[field].trim() !== "";

    const canSubmit = form.cdFilial && form.cdTipoOperacao && form.cdPlacaVeiculo && form.noMotorista;

    const handleSubmit = async () => {
        setTouched(new Set(["cdFilial", "cdTipoOperacao", "cdPlacaVeiculo", "noMotorista"]));
        if (!canSubmit) return;
        setLoading(true);
        setError(null);

        try {
            const payload = {
                cdFilial: Number(form.cdFilial),
                cdTipoOperacao: Number(form.cdTipoOperacao),
                cdPlacaVeiculo: form.cdPlacaVeiculo,
                noMotorista: form.noMotorista.trim(),
                ...(form.cdPedido ? { cdPedido: Number(form.cdPedido) } : {}),
                ...(form.noObservacao ? { noObservacao: form.noObservacao.trim() } : {}),
                dtFicha: new Date().toISOString().split('T')[0],
                cdPassoAtual: 1,
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

    // ── Tela de sucesso ──────────────────────────────────────────────────────
    if (success) {
        return (
            <div style={{
                display: "flex", flexDirection: "column", alignItems: "center",
                justifyContent: "center", height: 400, gap: 16
            }}>
                <div style={{
                    width: 64, height: 64, borderRadius: "50%",
                    background: "rgba(16,185,129,0.12)", border: "2px solid #10B981",
                    display: "flex", alignItems: "center", justifyContent: "center",
                    color: "#10B981",
                }}>
                    <Icon d={icons.check} size={28} />
                </div>
                <div style={{ fontSize: 18, fontWeight: 800, color: "#F9FAFB" }}>Ficha criada com sucesso!</div>
                <div style={{ fontSize: 13, color: "#6B7280" }}>Redirecionando para a listagem...</div>
            </div>
        );
    }

    // ── Formulário ───────────────────────────────────────────────────────────
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
                borderRadius: 10, overflow: "hidden", maxWidth: 720,
            }}>
                <div style={{ height: 3, background: "linear-gradient(90deg, #E85D04, #9A3412, transparent)" }} />

                <div style={{ padding: 32 }}>

                    {/* Seção 1 — Operação */}
                    <div style={{ marginBottom: 28 }}>
                        <div style={{
                            fontSize: 11, fontWeight: 700, color: "#E85D04",
                            textTransform: "uppercase", letterSpacing: "0.12em",
                            marginBottom: 18, display: "flex", alignItems: "center", gap: 8,
                        }}>
                            <Icon d={icons.ficha} size={13} /> Dados da Operação
                        </div>

                        <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 16 }}>
                            <Field label="Filial" icon={icons.map} required>
                                <select value={form.cdFilial} onChange={set("cdFilial")}
                                    style={{ ...inputStyle, borderColor: !isValid("cdFilial") ? "#EF4444" : "#374151", cursor: "pointer" }}>
                                    <option value="">Selecione a filial...</option>
                                    {filiais.map(f => <option key={f.value} value={f.value}>{f.label}</option>)}
                                </select>
                                {!isValid("cdFilial") && <span style={{ fontSize: 11, color: "#EF4444", marginTop: 4 }}>Campo obrigatório</span>}
                            </Field>

                            <Field label="Tipo de Operação" icon={icons.box} required>
                                <select value={form.cdTipoOperacao} onChange={set("cdTipoOperacao")}
                                    style={{ ...inputStyle, borderColor: !isValid("cdTipoOperacao") ? "#EF4444" : "#374151", cursor: "pointer" }}>
                                    <option value="">Selecione o tipo...</option>
                                    {tiposOperacao.map(t => <option key={t.value} value={t.value}>{t.label}</option>)}
                                </select>
                                {!isValid("cdTipoOperacao") && <span style={{ fontSize: 11, color: "#EF4444", marginTop: 4 }}>Campo obrigatório</span>}
                            </Field>
                        </div>
                    </div>

                    <div style={{ borderTop: "1px solid #1F2937", marginBottom: 28 }} />

                    {/* Seção 2 — Veículo e Motorista */}
                    <div style={{ marginBottom: 28 }}>
                        <div style={{
                            fontSize: 11, fontWeight: 700, color: "#E85D04",
                            textTransform: "uppercase", letterSpacing: "0.12em",
                            marginBottom: 18, display: "flex", alignItems: "center", gap: 8,
                        }}>
                            <Icon d={icons.truck} size={13} /> Veículo e Motorista
                        </div>

                        <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 16 }}>

                            {/* ── CAMPO PLACA — agora é SELECT dinâmico ── */}
                            <Field label="Placa do Veículo" icon={icons.truck} required>
                                <select
                                    value={form.cdPlacaVeiculo}
                                    onChange={set("cdPlacaVeiculo")}
                                    disabled={!form.cdFilial || loadingVeiculos}
                                    style={{
                                        ...inputStyle,
                                        borderColor: !isValid("cdPlacaVeiculo") ? "#EF4444" : "#374151",
                                        cursor: !form.cdFilial ? "not-allowed" : "pointer",
                                        opacity: !form.cdFilial ? 0.5 : 1,
                                        fontFamily: "'DM Mono', monospace",
                                        letterSpacing: "0.1em",
                                    }}
                                >
                                    {!form.cdFilial && <option value="">Selecione a filial primeiro...</option>}
                                    {form.cdFilial && loadingVeiculos && <option value="">Carregando veículos...</option>}
                                    {form.cdFilial && !loadingVeiculos && veiculos.length === 0 && (
                                        <option value="">Nenhum veículo cadastrado</option>
                                    )}
                                    {form.cdFilial && !loadingVeiculos && veiculos.length > 0 && (
                                        <>
                                            <option value="">Selecione a placa...</option>
                                            {veiculos.map(v => (
                                                <option key={v.cdPlacaVeiculo} value={v.cdPlacaVeiculo}>
                                                    {v.cdPlacaVeiculo}
                                                </option>
                                            ))}
                                        </>
                                    )}
                                </select>
                                {!isValid("cdPlacaVeiculo") && (
                                    <span style={{ fontSize: 11, color: "#EF4444", marginTop: 4 }}>Campo obrigatório</span>
                                )}
                                {form.cdFilial && !loadingVeiculos && veiculos.length === 0 && (
                                    <span style={{ fontSize: 11, color: "#F59E0B", marginTop: 4 }}>
                                        ⚠️ Cadastre veículos para esta filial antes de criar a ficha
                                    </span>
                                )}
                            </Field>

                            <Field label="Motorista" icon={icons.user} required>
                                <input value={form.noMotorista} onChange={set("noMotorista")}
                                    placeholder="Nome completo do motorista"
                                    style={{ ...inputStyle, borderColor: !isValid("noMotorista") ? "#EF4444" : "#374151" }} />
                                {!isValid("noMotorista") && <span style={{ fontSize: 11, color: "#EF4444", marginTop: 4 }}>Campo obrigatório</span>}
                            </Field>
                        </div>
                    </div>

                    <div style={{ borderTop: "1px solid #1F2937", marginBottom: 28 }} />

                    {/* Seção 3 — Informações adicionais */}
                    <div style={{ marginBottom: 32 }}>
                        <div style={{
                            fontSize: 11, fontWeight: 700, color: "#6B7280",
                            textTransform: "uppercase", letterSpacing: "0.12em",
                            marginBottom: 18, display: "flex", alignItems: "center", gap: 8,
                        }}>
                            <Icon d={icons.note} size={13} />
                            Informações Adicionais
                            <span style={{ fontSize: 10, fontWeight: 500, color: "#4B5563", textTransform: "none", letterSpacing: 0 }}>(opcional)</span>
                        </div>

                        <div style={{ display: "grid", gridTemplateColumns: "1fr 2fr", gap: 16 }}>
                            <Field label="Pedido Vinculado" icon={icons.box}>
                                <input value={form.cdPedido} onChange={set("cdPedido")}
                                    placeholder="Nº do pedido" type="number" min={1}
                                    style={{ ...inputStyle, fontFamily: "'DM Mono', monospace" }} />
                            </Field>

                            <Field label="Observação" icon={icons.note}>
                                <input value={form.noObservacao} onChange={set("noObservacao")}
                                    placeholder="Alguma informação adicional..." style={inputStyle} />
                            </Field>
                        </div>
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

                        <button onClick={handleSubmit} disabled={loading} style={{
                            background: loading ? "#374151" : "linear-gradient(135deg, #E85D04, #9A3412)",
                            border: "none", borderRadius: 6, color: "white", fontSize: 13, fontWeight: 700,
                            padding: "10px 24px", cursor: loading ? "not-allowed" : "pointer",
                            display: "flex", alignItems: "center", gap: 8,
                            transition: "opacity 0.15s", opacity: !canSubmit ? 0.6 : 1,
                        }}>
                            {loading ? (
                                <><Icon d={icons.loader} size={14} /> Criando ficha...</>
                            ) : (
                                <><Icon d={icons.check} size={14} /> Criar Ficha</>
                            )}
                        </button>
                    </div>

                </div>
            </div>
        </div>

    );

    // ─── Modal Novo Veículo ───────────────────────────────────────────────────────
    function ModalNovoVeiculo({
        cdFilial,
        onSalvo,
        onFechar,
    }: {
        cdFilial: string;
        onSalvo: (placa: string) => void;
        onFechar: () => void;
    }) {
        const [placa, setPlaca] = useState("");
        const [loading, setLoading] = useState(false);
        const [erro, setErro] = useState<string | null>(null);

        const handleSalvar = async () => {
            const placaFmt = placa.toUpperCase().trim();
            if (!placaFmt) return;

            setLoading(true);
            setErro(null);

            try {
                const res = await fetch(`${API}/api/Veiculo`, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ cdPlacaVeiculo: placaFmt, cdFilial: Number(cdFilial) }),
                });

                if (!res.ok) throw new Error("Erro ao cadastrar veículo");

                onSalvo(placaFmt);
            } catch (e: any) {
                setErro(e.message);
            } finally {
                setLoading(false);
            }
        };

        return (
            <>
                {/* Overlay */}
                <div onClick={onFechar} style={{
                    position: "fixed", inset: 0,
                    background: "rgba(0,0,0,0.6)", zIndex: 100,
                }} />

                {/* Modal */}
                <div style={{
                    position: "fixed", top: "50%", left: "50%",
                    transform: "translate(-50%, -50%)",
                    background: "#111827", border: "1px solid #374151",
                    borderRadius: 10, padding: 28, width: 360, zIndex: 101,
                    boxShadow: "0 20px 60px rgba(0,0,0,0.5)",
                }}>
                    {/* Topo */}
                    <div style={{ height: 3, background: "linear-gradient(90deg, #E85D04, #9A3412, transparent)", margin: "-28px -28px 24px" }} />

                    <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 20 }}>
                        <div>
                            <div style={{ fontSize: 15, fontWeight: 700, color: "#F9FAFB" }}>Cadastrar Veículo</div>
                            <div style={{ fontSize: 12, color: "#6B7280", marginTop: 2 }}>Filial selecionada: {cdFilial}</div>
                        </div>
                        <button onClick={onFechar} style={{
                            background: "transparent", border: "none", color: "#6B7280",
                            cursor: "pointer", padding: 4,
                        }}>
                            <Icon d={icons.close} size={16} />
                        </button>
                    </div>

                    <div style={{ marginBottom: 16 }}>
                        <label style={labelStyle}>
                            <Icon d={icons.truck} size={12} />
                            Placa do Veículo *
                        </label>
                        <input
                            value={placa}
                            onChange={e => { setPlaca(e.target.value); setErro(null); }}
                            onKeyDown={e => e.key === "Enter" && handleSalvar()}
                            placeholder="ABC-1234"
                            maxLength={8}
                            autoFocus
                            style={{
                                ...inputStyle,
                                fontFamily: "'DM Mono', monospace",
                                textTransform: "uppercase",
                                letterSpacing: "0.1em",
                            }}
                        />
                        {erro && (
                            <span style={{ fontSize: 11, color: "#EF4444", marginTop: 4, display: "block" }}>
                                {erro}
                            </span>
                        )}
                    </div>

                    <div style={{ display: "flex", gap: 8, justifyContent: "flex-end" }}>
                        <button onClick={onFechar} style={{
                            background: "transparent", border: "1px solid #374151", borderRadius: 6,
                            color: "#9CA3AF", fontSize: 12, fontWeight: 600, padding: "8px 16px", cursor: "pointer",
                        }}>
                            Cancelar
                        </button>
                        <button onClick={handleSalvar} disabled={!placa.trim() || loading} style={{
                            background: "linear-gradient(135deg, #E85D04, #9A3412)",
                            border: "none", borderRadius: 6, color: "white",
                            fontSize: 12, fontWeight: 700, padding: "8px 16px",
                            cursor: !placa.trim() || loading ? "not-allowed" : "pointer",
                            opacity: !placa.trim() ? 0.6 : 1,
                            display: "flex", alignItems: "center", gap: 6,
                        }}>
                            {loading
                                ? <><Icon d={icons.loader} size={12} /> Salvando...</>
                                : <><Icon d={icons.check} size={12} /> Salvar</>
                            }
                        </button>
                    </div>
                </div>
            </>
        );

        const [modalVeiculo, setModalVeiculo] = useState(false);

        {
            form.cdFilial && !loadingVeiculos && veiculos.length === 0 && (
                <div style={{ marginTop: 6, display: "flex", alignItems: "center", gap: 8 }}>
                    <span style={{ fontSize: 11, color: "#F59E0B" }}>
                        ⚠️ Nenhum veículo cadastrado para esta filial.
                    </span>
                    <button
                        onClick={() => setModalVeiculo(true)}
                        style={{
                            background: "transparent", border: "1px solid #E85D04",
                            borderRadius: 4, color: "#E85D04", fontSize: 11,
                            fontWeight: 700, padding: "2px 10px", cursor: "pointer",
                        }}
                    >
                        + Cadastrar
                    </button>


                    {modalVeiculo && (
                        <ModalNovoVeiculo
                            cdFilial={form.cdFilial}
                            onFechar={() => setModalVeiculo(false)}
                            onSalvo={(placa) => {
                                setVeiculos(v => [...v, { cdPlacaVeiculo: placa, cdFilial: Number(form.cdFilial) }]);
                                setForm(f => ({ ...f, cdPlacaVeiculo: placa }));
                                setModalVeiculo(false);
                            }}
                        />
                    )}

                </div>
            )
        }

    }
}