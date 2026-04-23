import { useState, useCallback, useMemo } from "react";
import NotaFiscalPage from "./NotaFiscalPage";
import { type Ficha, icons } from "./types";
import { useFichas } from "./useFichas";
import { Sidebar, Header, KPICard, PlaceholderPage, Spinner, ErrorBox, Icon } from "./ui";
import FichasPage from "./FichasPage";
import NovaFicha from "./NovaFicha";

function DashboardPage({ fichas, loading, error, reload }: {
    fichas: Ficha[]; loading: boolean; error: string | null; reload: () => void;
}) {
    const hoje = new Date().toISOString().slice(0, 10);
    const fichasHoje = useMemo(() => fichas.filter(f => f.dtFicha.slice(0, 10) === hoje), [fichas, hoje]);
    const emAndamento = useMemo(() => fichas.filter(f => f.status === "em_andamento"), [fichas]);
    const concluidas = useMemo(() => fichas.filter(f => f.status === "concluida"), [fichas]);

    return (
        <div>
            <div style={{ marginBottom: 24, display: "flex", justifyContent: "space-between", alignItems: "flex-start" }}>
                <div>
                    <h1 style={{ fontSize: 22, fontWeight: 800, color: "#F9FAFB", margin: 0, letterSpacing: "-0.03em" }}>Visão Geral</h1>
                    <p style={{ fontSize: 13, color: "#6B7280", margin: "4px 0 0" }}>Resumo operacional do dia</p>
                </div>
                <button onClick={reload} style={{
                    background: "#1F2937", border: "1px solid #374151", borderRadius: 6,
                    color: "#9CA3AF", fontSize: 12, padding: "7px 14px", cursor: "pointer", display: "flex", alignItems: "center", gap: 6
                }}>
                    <Icon d={icons.refresh} size={13} /> Atualizar
                </button>
            </div>
            {loading ? <Spinner /> : error ? <ErrorBox message={error} onRetry={reload} /> : (
                <>
                    <div style={{ display: "grid", gridTemplateColumns: "repeat(4, 1fr)", gap: 16, marginBottom: 28 }}>
                        <KPICard label="Total de Fichas" value={fichas.length} />
                        <KPICard label="Fichas Hoje" value={fichasHoje.length} />
                        <KPICard label="Em Andamento" value={emAndamento.length} color="#F59E0B" />
                        <KPICard label="Concluídas" value={concluidas.length} color="#10B981" />
                    </div>
                    <div style={{ background: "#111827", border: "1px solid #1F2937", borderRadius: 8, padding: 24 }}>
                        <div style={{ fontSize: 13, fontWeight: 700, color: "#D1D5DB", marginBottom: 16 }}>Últimas Fichas</div>
                        <div style={{ display: "flex", flexDirection: "column", gap: 10 }}>
                            {fichas.slice(0, 5).map(f => (
                                <div key={f.cdFicha} style={{
                                    display: "flex", alignItems: "center", gap: 12,
                                    padding: "10px 14px", background: "#0D1117", borderRadius: 6, border: "1px solid #1F2937"
                                }}>
                                    <span style={{ fontFamily: "'DM Mono', monospace", fontSize: 12, color: "#E85D04", fontWeight: 700, minWidth: 44 }}>
                                        #{String(f.cdFicha).padStart(4, "0")}
                                    </span>
                                    <span style={{ fontSize: 13, color: "#D1D5DB", flex: 1 }}>{f.noTipoOp} — {f.noFilial}</span>
                                </div>
                            ))}
                        </div>
                    </div>
                </>
            )}
        </div>
    );
}

export default function App() {
    const [active, setActive] = useState("dashboard");
    const [collapsed, setCollapsed] = useState(false);
    const [novaFichaAberta, setNovaFichaAberta] = useState(false);
    const { fichas, loading, error, reload } = useFichas();

    const handleSetActive = useCallback((key: string) => {
        setNovaFichaAberta(false);
        setActive(key);
    }, []);

    const handleNovaFichaSuccess = useCallback(() => {
        setNovaFichaAberta(false);
        reload();
    }, [reload]);

    const pageMap = useMemo(() => ({
        dashboard: <DashboardPage fichas={fichas} loading={loading} error={error} reload={reload} />,
        fichas: novaFichaAberta
            ? <NovaFicha onBack={() => setNovaFichaAberta(false)} onSuccess={handleNovaFichaSuccess} />
            : <FichasPage fichas={fichas} loading={loading} error={error} reload={reload} onNova={() => setNovaFichaAberta(true)} />,
        nf: <NotaFiscalPage />,
        pedidos: <PlaceholderPage label="Pedidos" />,
        veiculos: <PlaceholderPage label="Veículos" />,
        balanca: <PlaceholderPage label="Balanças" />,
        config: <PlaceholderPage label="Configurações" />,
    }), [fichas, loading, error, reload, novaFichaAberta, handleNovaFichaSuccess]);

    return (
        <>
            <style>{`
                @import url('https://fonts.googleapis.com/css2?family=DM+Mono:wght@400;500&family=Syne:wght@500;700;800&display=swap');
                * { box-sizing: border-box; margin: 0; padding: 0; }
                body { background: #060B10; font-family: 'Syne', sans-serif; color: #D1D5DB; }
                ::-webkit-scrollbar { width: 6px; }
                ::-webkit-scrollbar-track { background: #0D1117; }
                ::-webkit-scrollbar-thumb { background: #1F2937; border-radius: 3px; }
                input::placeholder { color: #4B5563; }
                select option { background: #111827; }
            `}</style>
            <div style={{ display: "flex", minHeight: "100vh", background: "#060B10" }}>
                <Sidebar active={active} setActive={handleSetActive} collapsed={collapsed} setCollapsed={setCollapsed} />
                <div style={{ flex: 1, display: "flex", flexDirection: "column", minWidth: 0, overflow: "hidden" }}>
                    <Header active={active} />
                    <main style={{ flex: 1, padding: 28, overflowY: "auto" }}>
                        {pageMap[active as keyof typeof pageMap]}
                    </main>
                    <footer style={{
                        borderTop: "1px solid #1F2937", padding: "10px 28px",
                        display: "flex", justifyContent: "space-between", alignItems: "center", background: "#0D1117"
                    }}>
                        <span style={{ fontSize: 11, color: "#374151" }}>RDS Forms Fábrica — v1.0.0</span>
                        <span style={{ fontSize: 11, color: "#374151", fontFamily: "'DM Mono', monospace" }}>
                            API: <span style={{ color: error ? "#EF4444" : "#10B981" }}>{error ? "● erro" : "● online"}</span>
                            &nbsp;|&nbsp; localhost:5186
                        </span>
                    </footer>
                </div>
            </div>
        </>
    );
}